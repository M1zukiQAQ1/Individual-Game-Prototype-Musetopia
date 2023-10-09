using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseStage : EnemyState
{

    private PlayerControllerInMap player;
    public EnemyChaseStage(EnemyControllerInMap mc, EnemyStateMachine esm) : base(mc, esm)
    {
        player = Object.FindObjectOfType<PlayerControllerInMap>();
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        enemy.MoveObject(player.transform.position);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
