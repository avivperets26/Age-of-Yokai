using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class AStar : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Tile[] tiles;

    private Node current;

    private int stepCount;

    private Stack<Vector3> path;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private HashSet<Vector3Int> changedTiles = new HashSet<Vector3Int>();

    private static HashSet<Vector3Int> noDiagonalTiles = new HashSet<Vector3Int>();

    

    //Test draw line
    LineRenderer LineRenderer;

    float distance;


    private Vector3Int startPos, goalPos;


    private void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();

        LineRenderer.positionCount = 2;

        changedTiles = new HashSet<Vector3Int>();
    }

    public Tilemap MyTilemap
    {
        get
        {
            return tilemap;
        }
    }

    public static HashSet<Vector3Int> NoDiagonalTiles
    {
        get
        {
            return noDiagonalTiles;
        }
    }

    public int MyStepCount
    {
        get
        {
            return stepCount;
        }

        set
        {
            stepCount = value;
        }
    }

    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        startPos = MyTilemap.WorldToCell(start);
        goalPos = MyTilemap.WorldToCell(goal);

        current = GetNode(startPos);

        //Creates an open list for nodes that we might want to look at later
        openList = new HashSet<Node>();

        //Creates a closed list for nodes that we have examined
        closedList = new HashSet<Node>();

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            node.Value.Parent = null;
        }

        allNodes.Clear();

        //Adds the current node to the open list (we have examined it)
        openList.Add(current);

        path = null;

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbours = FindNeighbours(current.Position);

            ExamineNeighbours(neighbours, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
        }

        

        if (path != null)
        {
            Stack<Vector3> ClearPath = new Stack<Vector3>();

            foreach (Vector3 position in path)//Paint Path
            {
                
                if (position != goalPos)
                {

                    //Debug.Log("Algorithm Vector3 position: "+position);

                    ClearPath.Push(position);

                    Vector3Int cellPosition = tilemap.WorldToCell(position);                    

                    tilemap.SetTile(cellPosition, tiles[0]);

                    
                }                
                
            }
            Debug.Log(ClearPath.Count);
            stepCount = ClearPath.Count - 1;

            while (ClearPath.Count != 0)
            {               
                //Debug.Log("Algorithm Vector3Int ClearPath.Count: " + ClearPath.Count);
                //Debug.Log("Algorithm Vector3Int clearPo ClearPath.Peek(): " + ClearPath.Peek());
                Vector3 clearPos = ClearPath.Pop();

                StartCoroutine(ClearPathWait(clearPos));
            }

            return path;
        }

        return null;
    }

    IEnumerator ClearPathWait(Vector3 clearPos)
    {

        yield return new WaitForSeconds(1.5f);

        Vector3Int cellPosition = tilemap.WorldToCell(clearPos);

        tilemap.SetTile(cellPosition, tiles[1]);

    }

    private List<Node> FindNeighbours(Vector3Int parentPosition)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) //These two forloops makes sure that we all nodes around our current node
        {
            for (int y = -1; y <= 1; y++)
            {
                if (y != 0 || x != 0)
                {
                    Vector3Int neighbourPosition = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                    if (neighbourPosition != startPos && !GameManager.MyInstance.MyBlocked.Contains(neighbourPosition))
                    {
                        Node neighbour = GetNode(neighbourPosition);
                        neighbours.Add(neighbour);
                    }

                }
            }
        }

        return neighbours;
    }

    private void ExamineNeighbours(List<Node> neighbours, Node current)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            Node neighbour = neighbours[i];

            if (!ConnectedDiagonally(current, neighbour))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbour.Position, current.Position);

            if (gScore == 14 && NoDiagonalTiles.Contains(neighbour.Position) && NoDiagonalTiles.Contains(current.Position))
            {
                continue;
            }

            if (openList.Contains(neighbour))
            {
                if (current.G + gScore < neighbour.G)
                {
                    CalcValues(current, neighbour, goalPos, gScore);
                }
            }
            else if (!closedList.Contains(neighbour))
            {
                CalcValues(current, neighbour, goalPos, gScore);

                if (!openList.Contains(neighbour)) //An extra check for openlist containing the neigbour
                {
                    openList.Add(neighbour); //Then we need to add the node to the openlist
                }
            }
        }
    }

    private bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        //Get's the direction
        Vector3Int direction = currentNode.Position - neighbour.Position;

        //Gets the positions of the nodes       
        Vector3Int first = new Vector3Int(currentNode.Position.x + (direction.x * -1), currentNode.Position.y, currentNode.Position.z);
        Vector3Int second = new Vector3Int(currentNode.Position.x, currentNode.Position.y + (direction.y * -1), currentNode.Position.z);

        //Checks if both nodes are empty
        if (GameManager.MyInstance.MyBlocked.Contains(first) || GameManager.MyInstance.MyBlocked.Contains(second))
        {
            return false;
        }

        //The ndoes are empty
        return true;
    }

    private int DetermineGScore(Vector3Int neighbour, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbour.x;
        int y = current.y - neighbour.y;

        if (Math.Abs(x - y) % 2 == 1)
        {
            gScore = 10; //The gscore for a vertical or horizontal node is 10
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        //The current node is removed fromt he open list
        openList.Remove(current);

        //The current node is added to the closed list
        closedList.Add(current);

        if (openList.Count > 0) //If the openlist has nodes on it, then we need to sort them by it's F value
        {
            current = openList.OrderBy(x => x.F).First();//Orders the list by the f value, to make it easier to pick the node with the lowest F val
        }
    }

    private Stack<Vector3> GeneratePath(Node current)
    {
        if (current.Position == goalPos) //If our current node is the goal, then we found a path
        {
            //Creates a stack to contain the final path
            Stack<Vector3> finalPath = new Stack<Vector3>();
            
            //Adds the nodes to the final path
            while (current != null)
            {
                //Debug.Log(current.Position);
                //DrawLine(current);

                //Adds the current node to the final path
                finalPath.Push(MyTilemap.CellToWorld(current.Position));
                //Find the parent of the node, this is actually retracing the whole path back to start
                //By doing so, we will end up with a complete path.
                current = current.Parent;
            }

            //Returns the complete path
            return finalPath;
        }

        return null;

    }

    private void DrawLine(Node current)
    {
        LineRenderer.SetPosition(0, new Vector3(current.Position.x - 1f, current.Position.y - 0.8899999f, 0f));
        //Debug.Log("current.Position: x: " + current.Position.x+ " y: "+ current.Position.y);

        LineRenderer.SetPosition(1, new Vector3(goalPos.x-1f, goalPos.y - 0.8899999f, 0f));

        //Debug.Log("goalPos: x: " + goalPos.x + " y: " + current.Position.y);

        distance = (goalPos - current.Position).magnitude;
    }

    private void CalcValues(Node parent, Node neighbour, Vector3Int goalPos, int cost)
    {
        
        //Sets the parent node
        neighbour.Parent = parent;

        //Calculates this nodes g cost, The parents g cost + what it costs to move tot his node
        neighbour.G = parent.G + cost;

        //H is calucalted, it's the distance from this node to the goal * 10
        neighbour.H = ((Math.Abs((neighbour.Position.x - goalPos.x)) + Math.Abs((neighbour.Position.y - goalPos.y))) * 10);

        //F is calcualted 
        neighbour.F = neighbour.G + neighbour.H;
    }



    private Node GetNode(Vector3Int position)
    {
        
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }

    //private void ChangeTile(Vector3Int clickPos)
    //{
    //    if (clickPos == startPos)
    //    {
    //        start = false;
    //        startPos = Vector3Int.zero;
    //    }
    //    else if (clickPos == goalPos)
    //    {
    //        goal = false;
    //        goalPos = Vector3Int.zero;
    //    }
    //    if (tileType == TileType.START)
    //    {
    //        if (start)
    //        {
    //            tilemap.SetTile(startPos, tiles[1]);
    //        }

    //        startPos = clickPos;
    //        start = true;

    //    }
    //    else if (tileType == TileType.GOAL)
    //    {
    //        if (goal)
    //        {
    //            tilemap.SetTile(goalPos, tiles[1]);
    //        }

    //        goalPos = clickPos;
    //        goal = true;
    //    }
    //    else if (tileType == TileType.BLOCKED)
    //    {
    //        blocked.Add(clickPos);
    //        tilemap.SetTile(clickPos, water);
    //    }
    //    else if (tileType == TileType.STANDARD)
    //    {
    //        blocked.Remove(clickPos);
    //    }

    //    if (tileType != TileType.BLOCKED)
    //    {
    //        tilemap.SetTile(clickPos, tiles[(int)tileType]);
    //    }

    //    changedTiles.Add(clickPos);

    //}

    //public void Erase()
    //{
    //    AstarDebugger.Instance.Erase(allNodes);

    //    foreach (Vector3 position in changedTiles)
    //    {

    //        Vector3Int cellPosition = tilemap.WorldToCell(position);

    //        tilemap.SetTile(cellPosition, tiles[1]);
                     
    //    }

    //    foreach (Vector3 path in path)
    //    {
    //        Vector3Int PathPosition = tilemap.WorldToCell(path);

    //        tilemap.SetTile(PathPosition, tiles[1]);
    //    }

    //    tilemap.SetTile(startPos, tiles[1]);

    //    tilemap.SetTile(goalPos, tiles[1]);

    //    allNodes.Clear();

    //    current = null;
    //}
}




public class Node
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public Node Parent { get; set; }
    public Vector3Int Position { get; set; }

    //private TextMeshProUGUI MyText { get; set; }

    public Node(Vector3Int position)
    {
        
        this.Position = position;
    }


}

