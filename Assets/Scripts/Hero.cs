using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{

    // Start is called before the first frame update
    [SerializeField]
    private Stat stamina;

    [SerializeField]
    private float initStamina = 100;

    protected override void Start()
    {
        stamina.Initialize(initStamina, initStamina);
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        //stamina.MyCurrentValue = 100;
        base.Update();
    }

    
    private void GetInput()
    {
        direction = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.I))
        {
            stamina.MyCurrentValue -= 10;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            stamina.MyCurrentValue += 10;
        }




        if (Input.GetKey(KeyCode.W))//UP
        {
            direction += Vector2.up;
           // targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetKey(KeyCode.S))//DOWN
        {
            direction += Vector2.down;
        }
        else if (Input.GetKey(KeyCode.D))//RIGHT
        {
            direction += Vector2.right;
        }
        else if (Input.GetKey(KeyCode.A))//LEFT
        {
            direction += Vector2.left;
        }
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
    }

}
