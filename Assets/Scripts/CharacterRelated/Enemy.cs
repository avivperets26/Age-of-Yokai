using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    /// <summary>
    /// A canvasgroup for the healthbar
    /// </summary>
    [SerializeField]
    private CanvasGroup healthGroup;

    // The enemys current state
    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    // The enemys attack range
    public float MyAttackRange { get; set; }

    // How much time has passed since the last attack
    public float MyAttackTime { get; set; }

    public Vector3 MyStartPosition { get; set; }

    [SerializeField]
    private float initAggroRange;

    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    protected void Awake()
    {
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 1;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {

            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();
        }

        base.Update();

    }

    /// <summary>
    /// When the enemy is selected
    /// </summary>
    /// <returns></returns>
    public override Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;

        return base.Select();
    }

    /// <summary>
    /// When we deselect our enemy
    /// </summary>
    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    /// <summary>
    /// Makes the enemy take damage when hit
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {
            SetTarget(source);

            base.TakeDamage(damage, source);

            OnHealthChanged(health.MyCurrentValue);
        }

    }

    /// <summary>
    /// Changes the enemys state
    /// </summary>
    /// <param name="newState">The new state</param>
    public void ChangeState(IState newState)
    {
        if (currentState != null) //Makes sure we have a state before we call exit
        {
            currentState.Exit();
        }

        //Sets the new state
        currentState = newState;

        //Calls enter on the new state
        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public override void Interact()
    {
        if (!IsAlive)
        {
            lootTable.ShowLoot();
        }
    }
}
