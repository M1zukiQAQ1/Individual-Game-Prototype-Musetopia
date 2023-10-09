using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackState : PlayerState
{

    private AnimatorStateInfo info;
    public PlayerAttackState(PlayerControllerInMap player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void EnterState()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        player.playerAnimator.SetTrigger("attack");
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        info = player.playerAnimator.GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime >= .6f && player.Enemy != null)
        {
            Debug.Log("Hit!");
            GameManagerInMap.instance.EnemyEncounter(player.Enemy, BattleEnterState.NormalEncounter);
            return;
        }

        if (!info.IsName("Attack"))
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
