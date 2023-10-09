using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscapeStateInFight : PlayerStateInFight
{
    public PlayerEscapeStateInFight(PlayerControllerInFight player, PlayerStateMachineInFight stateMachine) : base(player, stateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        player.targetPos = new Vector3(Mathf.Clamp(player.transform.position.x - Random.Range(2, 4), -8, 8), player.transform.position.y, player.transform.position.z);
        player.animator.SetBool("isMoving", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        player.animator.SetBool("isMoving", false);
    }

    public override void FrameUpdate()
    {
        player.MoveObject(player.targetPos);
        if (player.transform.position.Equals(player.targetPos))
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
