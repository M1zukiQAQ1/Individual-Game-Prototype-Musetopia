using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    // Start is called before the first frame update
    protected PlayerControllerInMap player;
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerControllerInMap player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() 
    {
//        player.rb2D.velocity = Vector2.zero;
    }

    public virtual void ExitState() { }
    public virtual void FrameUpdate() 
    {
        GameEventManager.instance.inputEvent.KeyPressed();
    }
    public virtual void AnimationUpdate() 
    {
        if (player.rb2D.velocity.x < 0)
        {
            player.transform.localScale = new Vector2(-1, 1);
        }
        else if(player.rb2D.velocity.x > 0)
        {
            player.transform.localScale = new Vector2(1, 1);
        }

        player.playerAnimator.SetFloat("moveSpeed", player.rb2D.velocity.magnitude);
    }
}
