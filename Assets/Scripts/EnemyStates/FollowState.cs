using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if(parent.Target != null)
        {
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;//find the target direction

            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.Target.position, parent.Speed * Time.deltaTime);//Moves the enemy towards the target
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }
}
