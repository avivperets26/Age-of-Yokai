using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer parentRenderer;

    private List<Obstacle> obstacles = new List<Obstacle>();
    
    // Start is called before the first frame update
    void Start()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)//When the player hits an obstacle
    {
        Debug.Log("Triger");
        if (collision.tag == "Obstacle")//If we hit an obstacle
        {
            Debug.Log("Colision Obstacle");
            Obstacle o = collision.GetComponent<Obstacle>();//Creates a reference to the obstacle

            o.FadeOut();

            if (obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder)//If we aren't colliding with anything else or we are colliding with
            {
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;//Change the sortorder to be behind waht we just hit
            }

            obstacles.Add(o);//Add the obstacle to the list, so that we can keep track of it
        }                
             
    }

    private void OnTriggerExit2D(Collider2D collision)//If we stopped colliding with an obstacle
    {
        if (collision.tag == "Obstacle")//If we stopped colliding with an obstacle
        {
            Obstacle o = collision.GetComponent<Obstacle>();//Creates a reference to the obstacle

            o.FadeIn();

            obstacles.Remove(o);//Removes the obstacle from the list

            if(obstacles.Count == 0)//We dont have any other obstacles
            {
                parentRenderer.sortingOrder = 200;
            }

            else
            {
                obstacles.Sort();

                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }
          
        }
            
    }
}
