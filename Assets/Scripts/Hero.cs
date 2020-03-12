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

    [SerializeField]
    private GameObject[] spellPrefab;
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
        if (Input.GetKeyDown(KeyCode.I))//Decrease Stamina by press I
        {
            stamina.MyCurrentValue -= 10;
        }
        else if (Input.GetKeyDown(KeyCode.O))//Increase Stamina by press O
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking && !IsMoving)
            {
                attackRoutine = StartCoroutine(Attack()); // Coroutine to attack at the same time of other functions, Not fully threading.
            }
        }
      
    }

    private IEnumerator Attack()//to Call Yield 
    {                        
        isAttacking = true;

        myAnimator.SetBool("attack", isAttacking);

        yield return new WaitForSeconds(1); //This is an hardcoded cast time, for debugging.

        Debug.Log("Attack Done");

        CastSpell();

        StopAttack();
    }

    public void CastSpell()
    {
        Instantiate(spellPrefab[0], transform.position, Quaternion.identity);//Make an instanse of Prefabe, position where it start, Quaternion to make sure the object will not rotate while mooving.
    }
}
