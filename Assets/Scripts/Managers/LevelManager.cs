using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform map;//A perent transform for our map, this will prevent our hierarchy to be flooded with tiles

    [SerializeField]
    private Texture2D[] mapData;// Map data fall all the map layers grass trees etc.

    [SerializeField]
    private MapElement[] mapElements;//A map element represent a tile that we can create in our game.

    [SerializeField]
    private Sprite defaultTile;//This title is ised for measuring the distance between tiles

    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();//Dictionary for all water tiles

    [SerializeField]
    private SpriteAtlas waterAtlas;

    private Vector3 WorldStartPos//The position of the bottom left corner of the screen
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateMap()//Generates our map
    {
        int height = mapData[0].height;
        int width = mapData[0].width;

        for (int i = 0; i < mapData.Length; i++)//Looks through all our map layers
        {
            for (int x = 0; x < mapData[i].width ; x++)//Runs through all pixels on the layer
            {
                for (int y = 0; y < mapData[i].height; y++)//Runs through all pixels on the layer
                {
                    Color c = mapData[i].GetPixel(x, y);                   

                    MapElement newElement = Array.Find(mapElements, e => e.Mycolor == c);//Checks if we have a tile that suits the color of the pixel on the map

                    if(newElement != null)//If we found an elemnt with the correct color
                    {
                        //Calculate x and y position of the title
                        float xPos = WorldStartPos.x + (defaultTile.bounds.size.x * x);

                        float yPos = WorldStartPos.y + (defaultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.MyElementPrefab);//Create the title

                        go.transform.position = new Vector2(xPos,yPos);//Set the titles position

                        if(newElement.MytileTag == "Water")//Checks if we are placing an Water
                        {
                            waterTiles.Add(new Point(x,y),go);//keys x and y have to be uniqe for the Dictionery call,
                        }

                        if (newElement.MytileTag == "Tree")//Checks if we are placing a tree
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height*2 - y*2;//IF we are placing a tree then we need to manage the order
                        }

                        go.transform.parent = map;//Make the title a child of map
                  
                    }
                }
            }
        }

        CheckWater();
    }

    public void CheckWater()
    {
        foreach (KeyValuePair<Point, GameObject> tile in waterTiles)
        {
            string composition = TileCheck(tile.Key);

            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("0");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("1");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("2");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("3");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("4");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("5");
            }
            if (composition[1] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("6");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("7");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("8");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("9");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("10");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("11");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("12");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("13");
            }
            if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("14");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("15");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("16");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("17");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 15)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("19");
                }
            }
            if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 10)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("20");
                }

            }

        }

    }

    public string TileCheck(Point currentPoint)//Check all neighbors of each tile, currentPosition = The position of the tile we are checking
    {
        string composition = string.Empty;//The composition that we are using

        for (int x = -1; x <= 1; x++)//Runs through all neighboors
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x != 0 || y != 0)
                {
                    if(waterTiles.ContainsKey(new Point(currentPoint.MyX+x,currentPoint.MyY+y)))//If x = -1 we will take the position from the left, If x= +1 we will take the position from the right
                    {
                        composition += "W";//W = Water tile
                    }
                    else
                    {
                        composition += "E";//E = Earth tile
                    }
                }
            }
        }

        Debug.Log(composition);

        return composition;
    }
}

[Serializable]
public class MapElement
{
    [SerializeField]
    private string tileTag;//This tile tag, used to check what tile we are placing

    [SerializeField]
    private Color color;//Tje color of the tiles, this is used to compare the tile with color on the map layer

    [SerializeField]
    private GameObject elemntPrefab;//Prefab that we use to spawn the tile in our world

    public GameObject MyElementPrefab//Property for accessing the refab
    {
        get
        {
            return elemntPrefab;
        }
    }

    public string MytileTag//Property for accessing the tag
    {
        get
        {
            return tileTag;
        }
    }

    public Color Mycolor//Property for accessing the color
    {
        get
        {
            return color;
        }
    }
}

public struct Point
{
    public int MyX { get; set; }
    public int MyY { get; set; }

    public Point(int x, int y)
    {
        this.MyX = x;
        this.MyY = y;
    }
}

