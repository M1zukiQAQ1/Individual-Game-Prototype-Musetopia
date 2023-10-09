using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerInMap : MonoBehaviour, IMovable, ITriggerable
{
    // Start is called before the first frame update

    public EnemyStateMachine StateMachine { get; set; }
    public MobIdleState IdleState { get; set; }
    public EnemyChaseStage ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyFightStage FightState { get; set; }

    public float moveSpeed { get; set; }
    public bool isTriggered { get; set; }

    [SerializeField] public EnemyInfo[] info;

    public void MoveObject(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
    }

    public void SetEnemyTriggered()
    {
        isTriggered = !isTriggered;
    }

    public int GetHighestEnemyLevel()
    {
        int highestLevel = int.MinValue;
        foreach(var enemy in info)
        {
            if(enemy.level > highestLevel)
            {
                highestLevel = enemy.level;
            }
        }
        return highestLevel;
    }

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();
        IdleState = new MobIdleState(this, StateMachine);
        ChaseState = new EnemyChaseStage(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        FightState = new EnemyFightStage(this, StateMachine);
    }

    private void Start()
    {
        isTriggered = false;
        moveSpeed = 6f;
        StateMachine.Initialize(IdleState);
    }


    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }
}
