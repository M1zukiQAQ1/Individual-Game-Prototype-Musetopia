using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobIdleState : EnemyState
{
    public MobIdleState(EnemyControllerInMap mc, EnemyStateMachine esm) : base(mc, esm)
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

    public override void FrameUpdate()
    {
        if (enemy.isTriggered)
        {
            enemyStateMachine.ChangeState(enemy.ChaseState);
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
