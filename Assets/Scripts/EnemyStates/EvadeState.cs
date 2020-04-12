using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{
    /// <summary>
    /// A reference to the enemy parent
    /// </summary>
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.Reset();
    }

    public void Update()
    {
        //Makes sure that we can run back to the original position when we are evading
        //This needs to be improved later so that we can use pathfinding
        parent.Direction = (parent.MyStartPosition - parent.transform.position).normalized;

        parent.transform.position = Vector2.MoveTowards
            (parent.transform.position, parent.MyStartPosition, parent.Speed * Time.deltaTime);

        //Calculates the distance between the enemy and the startpostion
        float distance = Vector2.Distance(parent.MyStartPosition, parent.transform.position);

        //If the distance is less t han 0 then we are back home and we need to idle
        if (distance <= 0)
        {
            parent.ChangeState(new IdleState());
        }
    }
}
