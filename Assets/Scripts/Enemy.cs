using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;//A canvasgroup for the health bar

    private IState currentState;//The enemy current state

    public float MyAttackRange { get; set; }//The enemy attack range

    public float MyAttackTime { get; set; }//How much time has passed since the last attack

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
        MyAggroRange = initAggroRange;

        MyAttackRange = 1;

        ChangeState(new IdleState());
    }

    protected override void Update()//Update is marked as virtual, so that we can override it in the subclasses
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

    public override Transform Select()//When the enemy is selected
    {
        healthGroup.alpha = 1;//Shows the health bar

        return base.Select();
    }

    public override void DeSelect()//When we deselct our enemy
    {
        healthGroup.alpha = 0;//Hides the healthbar

        base.DeSelect();
    }

    public override void TakeDamage(float damage, Transform source)//Makes the enemy take damage when hit
    {
        SetTarget(source);

        base.TakeDamage(damage,source);

        OnHealthChanged(health.MyCurrentValue);
    }


    public void ChangeState(IState newState)//Change Enemy state
    {
        if(currentState != null)//Makes sure we have a state before we call exit
        {
            currentState.Exit();
        }

        currentState = newState;//Sets the new state

        currentState.Enter(this);//Calls enter on the new state
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null)
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

        this.Myhealth.MyCurrentValue = this.Myhealth.MyMaxValue;

        OnHealthChanged(health.MyCurrentValue);
    }
}
