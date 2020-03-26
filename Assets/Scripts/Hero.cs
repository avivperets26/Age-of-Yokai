using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{

    // Start is called before the first frame update
    [SerializeField]
    private Stat stamina;

    [SerializeField]
    private float initStamina = 100; // The Player initial Stamina

    [SerializeField]
    private Block[] blocks; //An array of blocks used for blocking the player's sight.

    [SerializeField]
    private Transform[] exitPoints; // will store all the exit point of the wizard attack.

    private int exitIndex = 2;//will make sure we are using the right direction, initiate to 2 because defualt state is Down.

    private SpellBook spellBook;

    public Transform MyTarget { get; set; }//The Player target

    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
        stamina.Initialize(initStamina, initStamina);

        //For testing and debugging
        //target = GameObject.Find("Enemy Skeleton").transform;

        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        Debug.Log(LayerMask.GetMask("Block"));
        stamina.MyCurrentValue = 100;
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
            exitIndex = 0;
            direction += Vector2.up;
           // targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetKey(KeyCode.S))//DOWN
        {
            exitIndex = 2;
            direction += Vector2.down;
        }
        else if (Input.GetKey(KeyCode.D))//RIGHT
        {
            exitIndex = 1;
            direction += Vector2.right;
        }
        else if (Input.GetKey(KeyCode.A))//LEFT
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);

      
    }

    private IEnumerator Attack(int spellIndex)//To Call Yield 
    {
        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.CastSpell(spellIndex);

        isAttacking = true;//Indicates if we are attacking

        myAnimator.SetBool("attack", isAttacking); //Starts the attack animation

        yield return new WaitForSeconds(newSpell.MyCastTime); //This is an hardcoded cast time, for debugging.    
        
        if(currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();//Make an instanse of Prefabe, position where it start, Quaternion to make sure the object will not rotate while mooving.

            s.MyTarget = currentTarget;
        }
      
        StopAttack();//Ends the attack
    }

    public void CastSpell(int spellIndex)
    {
        Block();

        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight())//Check if we are able to attack
        {
            attackRoutine = StartCoroutine(Attack(spellIndex)); //Coroutine to attack at the same time of other functions, Not fully threading.
        }
       
    }

    private bool InLineOfSight()//Will check if we are in line of sight of our target
    {
        //Calculates the target's direction
        Vector3 targetDirecion = (MyTarget.transform.position - transform.position).normalized;

        //Throws a raycast in the direction of the target
        RaycastHit2D hit = Physics2D.Raycast(transform.position, MyTarget.position,Vector2.Distance(transform.position, MyTarget.transform.position),256);
        
        if(hit.collider == null)//If we didn't hit the block, then we can cast a spell
        {
            return true;
        }
        //If we hit the block we can't cast a spell
        return false;
    }

    private void Block()//Will Deactivate the block sight of the hero 
    {
        foreach (Block b in blocks)
        {
            b.Deactivete();
        }

        blocks[exitIndex].Activete();
    }

    public override void StopAttack()
    {
        spellBook.StopCasting();

        base.StopAttack();
    }
}
