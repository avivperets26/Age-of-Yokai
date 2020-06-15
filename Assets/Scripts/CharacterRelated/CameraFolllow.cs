using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFolllow : MonoBehaviour
{

    private Transform target;

    private float xMax, xMin, yMax, yMin;

    [SerializeField]
    private Tilemap tilemap;

    private Hero hero;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player1").transform;

        hero = target.GetComponent<Hero>();

        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);

        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

        SetLimits(minTile, maxTile);

        hero.SetLimits(minTile, maxTile);

    }

    private void LateUpdate()//will calls after all Update will finish calls, that means the camera will move after the player move.
    {
        int playerTurn = GameManager.MyInstance.MyPlayerTurn;
        if (playerTurn == 1)
        {
            target = GameObject.FindGameObjectWithTag("Player1").transform;

            
        }
        else if (playerTurn == 2)
        {
            target = GameObject.FindGameObjectWithTag("Player2").transform;

            
        }

        if (target.tag == "Player1")
        {
            //Debug.Log("CameraFollow: Player1 transform.position");
            transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10);
        }
        else if (target.tag == "Player2")
        {
            //Debug.Log("CameraFollow: Player2 transform.position");
            transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10);
        }       
    }

    private void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;

        float width = height * cam.aspect;

        xMin = minTile.x + width / 2;

        xMax = maxTile.x - width / 2;

        yMin = minTile.y + height / 2;

        yMax = maxTile.y - height / 2;
    }
}
