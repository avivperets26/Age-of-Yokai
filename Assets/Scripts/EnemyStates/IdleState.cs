using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IdleState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()//Called whenever we exit the state
    {
        
    }

    public void Update()//This is called as long as we are inside the state
    {
        Debug.Log("Idle");

        if (parent.Target != null)//If we have a targer, than we need to follow it.
        {
            parent.ChangeState(new FollowState());
        }
    }
}
