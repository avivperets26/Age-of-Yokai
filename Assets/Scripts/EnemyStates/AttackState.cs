using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;//A reference to the state parent

    private float attackCooldown= 3;

    private float extraRange = .1f;

    public void Enter(Enemy parent)//The state constructor
    {
        this.parent = parent;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        Debug.Log("Aattacking");

        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;

            parent.StartCoroutine(Attack());
        }

        if(parent.MyTarget != null)//If WE have a target then we need to check if we can attack it or if we need to follow
        {
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);//calculate the distance between the target and the enemy

            if(distance >= parent.MyAttackRange+extraRange && !parent.IsAttacking)//if the distance is larger then the attackrange, than we need to move
            {
                parent.ChangeState(new FollowState());//Follow the target
            }
        }
        else//If we lost the target then we need to idle
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }
}
