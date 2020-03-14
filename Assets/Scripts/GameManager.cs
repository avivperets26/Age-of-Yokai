using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Hero player;
    // Start is called before the first frame update
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
        if (Input.GetMouseButtonDown(0))//0 = Left Mouse button || 1 = Right Mouse button
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)
            {
                if(hit.collider.tag == "Enemy")
                {
                    player.MyTarget = hit.transform;
                }               
            }
            else
            {
                //Detarget the target
                player.MyTarget = null;
            }
        }
        

        
    }
}
