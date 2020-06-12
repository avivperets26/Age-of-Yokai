using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarDebugger : MonoBehaviour
{
    private static AstarDebugger instance;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Tile tile;

    [SerializeField]
    private Color openColor, closedColor, pathColor, currentColor, startColor, goalColor;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject debugTextPrefab;

    List<GameObject> debugObjects = new List<GameObject>();

    public static AstarDebugger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AstarDebugger>();
            }

            return instance;
        }
    }

    public void CreateTiles(HashSet<Node> open, HashSet<Node> closed, Dictionary<Vector3Int, Node> allNodes, Vector3Int current, Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {

        foreach (GameObject gameObject in debugObjects)
        {
            Destroy(gameObject);
        }

        debugObjects.Clear();

        foreach (Node node in open)
        {
            if (node.Position != start && node.Position != goal)
            {
                ColorTile(node.Position, openColor);
            }
        }

        foreach (Node node in closed)
        {
            if (node.Position != start && node.Position != goal)
            {
                ColorTile(node.Position, closedColor);
            }
        }

        if (path != null)
        {
            foreach (Vector3Int pos in path)
            {
                if (pos != start && pos != goal)
                {
                    ColorTile(pos, pathColor);
                }

            }
        }

        ColorTile(current, currentColor);
        ColorTile(start, startColor);
        ColorTile(goal, goalColor);

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            if (node.Value.Parent != null)
            {
                GameObject go = Instantiate(debugTextPrefab, canvas.transform);                
                go.transform.position = grid.CellToWorld(node.Key);
                debugObjects.Add(go);
                GenerateDebugText(node.Value, go.GetComponent<DebugText>());
            }
     
        }
    }

    private void GenerateDebugText(Node node, DebugText debugText)
    {
        debugText.P.text = $"P:{node.Position.x},{node.Position.y}";

        debugText.F.text = $"F:{node.F}";

        debugText.G.text = $"G:{node.G}";

        debugText.H.text = $"H:{node.H}";

        if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 135));
        }
        else if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 225));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
        else if (node.Parent.Position.x == node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (node.Parent.Position.x == node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }

    }

    private void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }

    public void Erase(Dictionary<Vector3Int, Node> allNodes)
    {
        foreach (GameObject gameObject in debugObjects)
        {
            Destroy(gameObject);
        }

        debugObjects.Clear();

        foreach (Vector3Int position in allNodes.Keys)
        {
            tilemap.SetTile(position, null);
        }
    }

    public void ShowHide()
    {
        canvas.gameObject.SetActive(!canvas.isActiveAndEnabled);
        Color c = tilemap.color;
        c.a = c.a != 0 ? 0 : 1;
        tilemap.color = c;
    }
}
