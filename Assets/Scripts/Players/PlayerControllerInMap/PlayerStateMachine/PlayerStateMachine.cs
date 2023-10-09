using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; set; }
    public void Initialize(PlayerState currentState)
    {
        this.currentState = currentState;
        this.currentState.EnterState();
    }

    public void ChangeState(PlayerState nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        nextState.EnterState();
    }

}
