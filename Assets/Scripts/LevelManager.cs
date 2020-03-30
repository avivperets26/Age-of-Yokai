using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform map;

    [SerializeField]
    private Texture2D[] mapData;

    [SerializeField]
    private MapElement[] mapElements;

    [SerializeField]
    private Sprite defaultTile;

    private Vector3 WorldStartPos
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

                    Debug.Log(ColorUtility.ToHtmlStringRGBA(c));//Gets the color of the current pixel

                    MapElement newElement = Array.Find(mapElements, e => e.Mycolor == c);//Checks if we have a tile that suits the color of the pixel on the map

                    if(newElement != null)//If we found an elemnt with the correct color
                    {
                        //Calculate x and y position of the title
                        float xPos = WorldStartPos.x + (defaultTile.bounds.size.x * x);

                        float yPos = WorldStartPos.y + (defaultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.MyElementPrefab);//Create the title

                        go.transform.position = new Vector2(xPos,yPos);//Set the titles position

                        if (newElement.MytileTag == "Tree")//Checks if we are placing a tree
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height*2 - y*2;//IF we are placing a tree then we need to manage the 
                        }

                        go.transform.parent = map;//Make the title a child of map
                  
                    }
                }
            }
        }
    }
}

[Serializable]
public class MapElement
{
    [SerializeField]
    private string tileTag;

    [SerializeField]
    private Color color;

    [SerializeField]
    private GameObject elemntPrefab;

    public GameObject MyElementPrefab
    {
        get
        {
            return elemntPrefab;
        }
    }

    public string MytileTag
    {
        get
        {
            return tileTag;
        }
    }

    public Color Mycolor
    {
        get
        {
            return color;
        }
    }
}
