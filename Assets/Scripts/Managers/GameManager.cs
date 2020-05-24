using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// A reference to the player object
    /// </summary>
    [SerializeField]
    private Hero player;

    private Enemy currentTarget;

    // Update is called once per frame
    void Update()
    {
        //Executes click target
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())//If we click the left mouse button
        {
            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null && hit.collider.tag =="Enemy")//If we hit something
            {
                if (currentTarget != null)//If we have a current target
                {
                    currentTarget.DeSelect(); //deselct the current target
                }

                currentTarget = hit.collider.GetComponent<Enemy>(); //Selects the new target

                player.MyTarget = currentTarget.Select(); //Gives the player the new target

                UIManager.MyInstance.ShowTargetFrame(currentTarget);
            }
            else//Deselect the target
            {
                UIManager.MyInstance.HideTargetFrame();

                if (currentTarget != null) //If we have a current target
                {
                    currentTarget.DeSelect(); //We deselct it
                }

                //We remove the references to the target
                currentTarget = null;
                player.MyTarget = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && hit.collider.gameObject.GetComponent<IInteractable>() == player.MyInteractable)
            {
                player.Interact();
            }
        }

    }
}
