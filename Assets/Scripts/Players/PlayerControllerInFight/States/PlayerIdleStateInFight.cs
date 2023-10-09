using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleStateInFight : PlayerStateInFight
{
    public PlayerIdleStateInFight(PlayerControllerInFight player, PlayerStateMachineInFight stateMachine) : base(player, stateMachine)
    {
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        switch (player.role)
        {
            case ROLE.WARRIOR:
                if (GameManager.instance.GetJudgers()[(int)player.role].GetComboCounter() % player.comboToAttack == 0 && player.lastComboNum != GameManager.instance.GetJudgers()[(int)player.role].GetComboCounter())
                {
                    player.lastComboNum = GameManager.instance.GetJudgers()[(int)player.role].GetComboCounter();
                    player.target = GameManager.instance.GetMobByAttackSequence().transform;
                    player.targetPos = player.target.position - new Vector3(1, 0, 0);

                    stateMachine.ChangeState(player.chaseState);
                }
                break;
            case ROLE.HEALER:
                if (GameManager.instance.GetJudgers()[(int)player.role].GetComboCounter() % player.comboToAttack == 0 && player.lastComboNum != GameManager.instance.GetJudgers()[(int)player.role].GetComboCounter())
                {
                    player.lastComboNum = GameManager.instance.GetJudgers()[(int)player.role].GetComboCounter();

                    player.target = GameManager.instance.GetPlayerWithLowestHealth(true).transform;
                    player.targetPos = player.target.position;
                    player.targetPos = player.target.position - new Vector3(1, 0, 0);

                    stateMachine.ChangeState(player.chaseState);
                }
                break;
            default:
                break;
        }
        
    }
}
