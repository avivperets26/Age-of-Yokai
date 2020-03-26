using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Hero player;
    // Start is called before the first frame update

    private NPC currentTarget;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    public void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())//0 = Left Mouse button || 1 = Right Mouse button //EventSystem.current.IsPointerOverGameObject() == if mouse is over Button code not excute.
        {
            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)// if we hiy something
            {
                if(currentTarget != null)
                {
                    currentTarget.DeSelect();
                }

                currentTarget = hit.collider.GetComponent<NPC>();

                player.MyTarget = currentTarget.Select();
            }
            else
            {
                if(currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = null;
                player.MyTarget = null;
            }

        }
        

        
    }
}
