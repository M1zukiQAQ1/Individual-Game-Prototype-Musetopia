using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChaseStateInFight : PlayerStateInFight
{
    public PlayerChaseStateInFight(PlayerControllerInFight player, PlayerStateMachineInFight stateMachine) : base(player, stateMachine)
    {
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
    }

    public override void EnterState()
    {
        base.EnterState();
        player.animator.SetBool("isMoving", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        player.animator.SetBool("isMoving", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        player.MoveObject(player.targetPos);
        if (player.transform.position.Equals(player.targetPos))
        {
            player.stateMachine.ChangeState(player.attackState);
        }
    }
}
