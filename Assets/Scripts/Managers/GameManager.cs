using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{



    [SerializeField]
    private Hero player;
    

    private NPC currentTarget;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();//Executes click target
    }

    public void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())//0 = Left Mouse button || 1 = Right Mouse button //EventSystem.current.IsPointerOverGameObject() == if mouse is over Button code not excute.
        {
            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)// if we hiy something
            {
                if(currentTarget != null)//if we  have a current target
                {
                    currentTarget.DeSelect();//Deselecet the current target
                }

                currentTarget = hit.collider.GetComponent<NPC>();//Selects the new target

                player.MyTarget = currentTarget.Select();//Gives the player new target

                UIManager.MyInstance.ShowTargetFrame(currentTarget);
            }
            else//Deselect the target
            {
                UIManager.MyInstance.HideTargetFrame();

                if(currentTarget != null)//If we have a current target
                {
                    currentTarget.DeSelect();//We deselect it
                }

                //Removing the references to the target
                currentTarget = null;
                player.MyTarget = null;
            }

        }
        

        
    }
}
