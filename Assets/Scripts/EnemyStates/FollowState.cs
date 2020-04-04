using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FollowState : IState//The enemy follow state
{
    private Enemy parent;//A reference to the parent
    public void Enter(Enemy parent)//Called whenever we enter the state
    {
        this.parent = parent;
    }

    public void Exit()//This is called whenever we exit the state
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()//This is called as long as we are inside the state
    {
        Debug.Log("Follow");

        if (parent.MyTarget != null)//As long we have a target, then we need to keep moving
        {
            parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;//find the target direction

            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyTarget.position, parent.Speed * Time.deltaTime);//Moves the enemy towards the target

            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

            if(distance<= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        if (!parent.InRange)//we dont have a target, then we need to go back to idle
        {
            parent.ChangeState(new EvadeState());
        }       
    }
}
