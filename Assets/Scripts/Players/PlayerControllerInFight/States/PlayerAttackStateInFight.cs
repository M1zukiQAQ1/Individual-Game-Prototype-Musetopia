using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStateInFight : PlayerStateInFight
{
    public PlayerAttackStateInFight(PlayerControllerInFight player, PlayerStateMachineInFight stateMachine) : base(player, stateMachine)
    {
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
    }

    public override void EnterState()
    {
        base.EnterState();
        player.StartCoroutine(DealDamage());
    }

    private IEnumerator DealDamage()
    {
        Debug.Log("Attack animation triggered");
        player.animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.6f);

        double damage = player.GetDamage();
        switch (player.role)
        {
            case ROLE.WARRIOR:
                player.totalDamageDelt += damage;
                player.target.GetComponent<MobController>().TakeDamage(damage);
                break;
            case ROLE.HEALER:
                damage *= -1;
                player.totalDamageDelt += Mathf.Abs((float)damage);
                player.target.GetComponent<PlayerControllerInFight>().TakeDamage(damage);
                break;
            default:
                Debug.Log("Error!");
                break;
        }

        yield return new WaitForSeconds(2f);

        player.stateMachine.ChangeState(player.escapeState);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

}
