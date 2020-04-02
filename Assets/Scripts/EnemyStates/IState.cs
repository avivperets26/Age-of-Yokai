using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter(Enemy parent);//Prepare the State

    void Update();

    void Exit();//Will clean everything this state did.
}
