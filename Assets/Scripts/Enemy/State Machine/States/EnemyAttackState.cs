using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyControllerInMap mc, EnemyStateMachine esm) : base(mc, esm)
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

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
