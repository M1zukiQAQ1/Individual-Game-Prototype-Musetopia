using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachineInFight
{
    public PlayerStateInFight currentState { get; set; }
    public void Initialize(PlayerStateInFight currentState)
    {
        Debug.Log("Initialized to " + currentState);
        this.currentState = currentState;
        this.currentState.EnterState();
    }

    public void ChangeState(PlayerStateInFight nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        nextState.EnterState();
    }
}
