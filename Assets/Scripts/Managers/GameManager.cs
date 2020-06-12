using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{

    public event KillConfirmed killConfirmedEvent;

    private Camera mainCamera;

    private static GameManager instance;

    /// <summary>
    /// A reference to the player object
    /// </summary>
    [SerializeField]
    private Hero player;

    [SerializeField]
    private LayerMask clickableLayer, groundLayer;

    private Enemy currentTarget;

    private int targetIndex;

    private HashSet<Vector3> blocked = new HashSet<Vector3>();

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    public HashSet<Vector3> MyBlocked
    {
        get
        {
            return blocked;
        }

        set
        {
            blocked = value;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Executes click target
        ClickTarget();

        NextTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())//If we click the left mouse button
        {
            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null && hit.collider.tag =="Enemy")//If we hit something
            {
                DeSelecetTarget();

                SelectTarget(hit.collider.GetComponent<Enemy>());

            }
            else//Deselect the target
            {
                UIManager.MyInstance.HideTargetFrame();

                DeSelecetTarget();

                //We remove the references to the target
                currentTarget = null;
                player.MyTarget = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);
            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

                if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
                {
                    entity.Interact();
                }
            }          
            else
            {
                hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);

                if (hit.collider != null)
                {                  
                    player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                }
            }
        }
    }

    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeSelecetTarget();

            if (Hero.MyInstance.MyAttackers.Count > 0)
            {
                if (targetIndex < Hero.MyInstance.MyAttackers.Count)
                {
                    SelectTarget(Hero.MyInstance.MyAttackers[targetIndex]);

                    targetIndex++;

                    if (targetIndex >= Hero.MyInstance.MyAttackers.Count)
                    {
                        targetIndex = 0;
                    }
                }
                else
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void DeSelecetTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.DeSelect();
        }
    }

    private void SelectTarget(Enemy enemy)
    {
        currentTarget = enemy;

        player.MyTarget = currentTarget.Select();

        UIManager.MyInstance.ShowTargetFrame(currentTarget);
    }

    public void OnKillConfirmed(Character character)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }
}
