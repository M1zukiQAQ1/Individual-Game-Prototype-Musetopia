using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateInFight
{
    // Start is called before the first frame update
    protected PlayerControllerInFight player;
    protected PlayerStateMachineInFight stateMachine;

    public PlayerStateInFight(PlayerControllerInFight player, PlayerStateMachineInFight stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() 
    {
        if(player.role == ROLE.WARRIOR)
        {
            Debug.Log("Entered state " + this);
        }
    }
    public virtual void ExitState() 
    {
        if (player.role == ROLE.WARRIOR)
        {
            Debug.Log("Existed state " + this);
        }
    }
    public virtual void FrameUpdate() { }
    public virtual void AnimationUpdate() 
    {
        if(player.role != ROLE.DEFENDER)
        {
            if (player.targetPos.x < player.transform.position.x)
            {
                player.transform.localScale = new Vector3(-1, 1, 1);
            }

            else
            {
                player.transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }
}
