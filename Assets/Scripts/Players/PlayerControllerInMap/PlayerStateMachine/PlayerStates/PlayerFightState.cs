using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFightState : PlayerState
{
    // Start is called before the first frame update
    public PlayerFightState(PlayerControllerInMap player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {
        
    }

}
