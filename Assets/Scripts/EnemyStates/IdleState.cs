using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IdleState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)//This is callled whenever we eneter the state
    {
        this.parent = parent;

        this.parent.Reset();
    }

    public void Exit()//Called whenever we exit the state
    {
        
    }

    public void Update()//This is called as long as we are inside the state
    {
        Debug.Log("Idle");

        if (parent.MyTarget != null)//If we have a targer, than we need to follow it.
        {
            parent.ChangeState(new FollowState());
        }
    }
}
