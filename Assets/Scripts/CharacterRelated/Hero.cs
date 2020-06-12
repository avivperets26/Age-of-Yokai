using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the player script, it contains functionality that is specific to the Player
/// </summary>
public class Hero : Character
{
    private static Hero instance;

    public static Hero MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Hero>();
            }

            return instance;
        }
    }

    private List<Enemy> attackers = new List<Enemy>();

    /// <summary>
    /// The player's mana
    /// </summary>
    [SerializeField]
    private Stat stamina;

    [SerializeField]//The player Xp
    private Stat xpStat;

    [SerializeField]
    private Text levelText;

    /// <summary>
    /// The player's initial mana
    /// </summary>
    private float initStamina = 50;

    private Vector2 initPos;

    /// <summary>
    /// An array of blocks used for blocking the player's sight
    /// </summary>
    [SerializeField]
    private Block[] blocks;

    /// <summary>
    /// Exit points for the spells
    /// </summary>
    [SerializeField]
    private Transform[] exitPoints;

    [SerializeField]
    private Animator ding;

    [SerializeField]
    private Transform minimapIcon;

    /// <summary>
    /// Index that keeps track of which exit point to use, 2 is default down
    /// </summary>
    private int exitIndex = 2;

    public Coroutine MyInitRoutine { get; set; }

    private List<IInteractable> interactables = new List<IInteractable>();

    #region PATHFINDING

    private Vector3 destination;

    private Vector3 current;

    private Vector3 goal;

    [SerializeField]
    private AStar astar;
    #endregion

    private Vector3 min, max;

    public int MyGold { get; set; }
    public List<IInteractable> MyInteractables { get => interactables; set => interactables = value; }

    public Stat MyXp
    {
        get
        {
            return xpStat;
        }

        set
        {
            xpStat = value;
        }
    }

    public Stat MyStamina
    {
        get
        {
            return stamina;
        }

        set
        {
            stamina = value;
        }
    }

    public List<Enemy> MyAttackers
    {
        get
        {
            return attackers;
        }

        set
        {
            attackers = value;
        }
    }



    /// <summary>
    /// We are overriding the characters update function, so that we can execute our own functions
    /// </summary>
    protected override void Update()
    {
        //Executes the GetInput function
        GetInput();

        ClickToMove();

        //Clamps the player inside the tilemap
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y),
            transform.position.z);

        base.Update();
    }

    protected void FixedUpdate()
    {
        Move();
    }

    public void SetDefaultValues()
    {
        MyGold = 1000;

        health.Initialize(initHealth, initHealth);

        MyStamina.Initialize(initStamina, initStamina);

        MyXp.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));//y=100*x*x^0.5 Algorithm

        levelText.text = MyLevel.ToString();

        initPos = transform.parent.position;
    }

    /// <summary>
    /// Listen's to the players input
    /// </summary>
    private void GetInput()
    {
        Direction = Vector2.zero;

        ///THIS IS USED FOR DEBUGGING ONLY
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            MyStamina.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(10);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            MyStamina.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeyBindManager.MyInstance.Keybinds["UP"])) //Moves up
        {
            exitIndex = 0;
            Direction += Vector2.up;
            minimapIcon.eulerAngles = new Vector3(0,0,0);
      
        }
        if (Input.GetKey(KeyBindManager.MyInstance.Keybinds["LEFT"])) //Moves left
        {
            exitIndex = 3;
            Direction += Vector2.left;
            if (Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 90);
            }
        }
        if (Input.GetKey(KeyBindManager.MyInstance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            Direction += Vector2.down;

            minimapIcon.eulerAngles = new Vector3(0, 0, 180);
        }
        if (Input.GetKey(KeyBindManager.MyInstance.Keybinds["RIGHT"])) //Moves right
        {
            exitIndex = 1;
            Direction += Vector2.right;
            if (Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 270);
            }
        }
        if (IsMoving)
        {
            StopAction();

            StopInit();
        }

        foreach (string action in KeyBindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeyBindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);

            }
        }


    }

    /// <summary>
    /// Set's the player's limits so that he can't leave the game world
    /// </summary>
    /// <param name="min">The minimum position of the player</param>
    /// <param name="max">The maximum postion of the player</param>
    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// A co routine for attacking
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackRoutine(ICastable castable)
    {
        Transform currentTarget = MyTarget;

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if (currentTarget != null && InLineOfSight())
        {
            Spell newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);

            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.MyDamage, transform);
        }

        StopAction(); //Ends the attack
    }
    private IEnumerator GatherRoutine(ICastable castable, List<Drop> items)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable)); //This is a hardcoded cast time, for debugging      

        LootWindow.MyInstance.CreatePages(items);
    }

    private IEnumerator ActionRoutine(ICastable castable)
    {
        SpellBook.MyInstance.Cast(castable);

        IsAttacking = true; //Indicates if we are attacking

        MyAnimator.SetBool("attack", IsAttacking); //Starts the attack animation

        yield return new WaitForSeconds(castable.MyCastTime);

        StopAction();
    }

    /// <summary>
    /// Casts a spell
    /// </summary>
    public void CastSpell(ICastable castable)
    {
        Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight()) //Chcks if we are able to attack
        {
            MyInitRoutine = StartCoroutine(AttackRoutine(castable));
        }
    }

    public void Gather(ICastable castable, List<Drop> items)
    {
        if (!IsAttacking)
        {
            MyInitRoutine = StartCoroutine(GatherRoutine(castable, items));
        }
    }

    /// <summary>
    /// Checks if the target is in line of sight
    /// </summary>
    /// <returns></returns>
    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            //Calculates the target's direction
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            //Thorws a raycast in the direction of the target
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            //If we didn't hit the block, then we can cast a spell
            if (hit.collider == null)
            {
                return true;
            }

        }

        //If we hit the block we can't cast a spell
        return false;
    }

    /// <summary>
    /// Changes the blocks based on the players direction
    /// </summary>
    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }

    /// <summary>
    /// Stops the attack
    /// </summary>
    public void StopAction()
    {

        //Stop the spellbook from casting
        SpellBook.MyInstance.StopCating();

        IsAttacking = false; //Makes sure that we are not attacking

        MyAnimator.SetBool("attack", IsAttacking); //Stops the attack animation

        if (actionRoutine != null) //Checks if we have a reference to an co routine
        {
            StopCoroutine(actionRoutine);
        }
    }

    private void StopInit()
    {
        if (MyInitRoutine != null)
        {
            StopCoroutine(MyInitRoutine);
        }
    }

    public void GainXP(int xp)
    {
        MyXp.MyCurrentValue += xp;

        CombatTextManager.MyInstace.CreatText(transform.position, xp.ToString(), SCTTYPE.XP, false);

        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy))
        {
            MyAttackers.Add(enemy);
        }
    }

    private IEnumerator Ding()
    {
        while (!MyXp.IsFull)
        {
            yield return null;
        }

        MyLevel++;

        ding.SetTrigger("Ding");

        levelText.text = MyLevel.ToString();

        MyXp.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);

        MyXp.MyMaxValue = Mathf.Floor(MyXp.MyMaxValue);

        MyXp.MyCurrentValue = MyXp.MyOverflow;//Keeps the rest of the xp after level up

        MyXp.Reset();

        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }
    
    public void GetPath(Vector3 goal)
    {
        Debug.Log("Hero Get Path:" + goal);
        MyPath = astar.Algorithm(transform.position, goal);

        current = MyPath.Pop();

        destination = MyPath.Pop();

        this.goal = goal;
    }

    public IEnumerator Respawn()
    {
        MySpriteRenderer.enabled = false;

        yield return new WaitForSeconds(5f);

        health.Initialize(initHealth, initHealth);

        MyStamina.Initialize(initStamina, initStamina);

        transform.parent.position = initPos;

        MySpriteRenderer.enabled = true;

        MyAnimator.SetTrigger("respawn");
    }

    private void ClickToMove()
    {
        if (MyPath != null)
        {
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, Speed * Time.deltaTime);

            Vector3Int dest = astar.MyTilemap.WorldToCell(destination);

            Vector3Int cur = astar.MyTilemap.WorldToCell(current);

            float distance = Vector2.Distance(destination, transform.parent.position);

            if (cur.y > dest.y)
            {
                Direction = Vector2.down;
            }
            else if (cur.y < dest.y)
            {
                Direction = Vector2.up;
            }
            if (cur.y == dest.y)
            {

                if (cur.x > dest.x)
                {
                    Direction = Vector2.left;
                }
                else if (cur.x < dest.x)
                {
                    Direction = Vector2.right;
                }
            }

            if (distance <= 0f)
            {
                if (MyPath.Count > 0)
                {
                    current = destination;

                    destination = MyPath.Pop();
                }
                else
                {
                    MyPath = null;
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {

            IInteractable interactable = collision.GetComponent<IInteractable>();

            if (!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if (MyInteractables.Count >0)
            {
                IInteractable interactable = MyInteractables.Find(x => x == collision.GetComponent<IInteractable>());

                if (interactable != null)
                {
                    interactable.StopInteract();
                }

                MyInteractables.Remove(interactable);
            }
        }
    }
}
