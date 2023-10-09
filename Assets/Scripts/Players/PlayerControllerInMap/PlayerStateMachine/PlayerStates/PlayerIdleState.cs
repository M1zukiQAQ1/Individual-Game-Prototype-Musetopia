using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{

    public PlayerIdleState(PlayerControllerInMap player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void EnterState()
    {

    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if(player.canJump && Input.GetKeyDown(KeyCode.Space)){
            player.rb2D.velocity += new Vector2(0, player.playerJumpVelocity);
            player.canJump = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            stateMachine.ChangeState(player.AttackState);
            return;
        }

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }

}
