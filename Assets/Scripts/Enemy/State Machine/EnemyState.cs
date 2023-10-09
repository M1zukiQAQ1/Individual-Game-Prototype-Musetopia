using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    // Start is called before the first frame update

    protected EnemyControllerInMap enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyControllerInMap mc, EnemyStateMachine esm)
    {
        enemy = mc;
        enemyStateMachine = esm;
    }

    public virtual void EnterState() { }

    public virtual void ExitState() { }
    
    public virtual void FrameUpdate() { }

    public virtual void PhysicsUpdate() { }
}
