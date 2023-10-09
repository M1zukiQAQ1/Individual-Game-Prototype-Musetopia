using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerControllerInMap player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (player.canJump && Input.GetKeyDown(KeyCode.Space)){
            player.rb2D.velocity += new Vector2(0, player.playerJumpVelocity);
            player.canJump = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            stateMachine.ChangeState(player.AttackState);
            return;
        }

        player.rb2D.velocity = new Vector2(Input.GetAxis("Horizontal") * player.moveSpeed, player.rb2D.velocity.y);

        if (player.rb2D.velocity.magnitude== 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
