using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInMap : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb2D;
    public float moveSpeed = 5f;
    public Animator playerAnimator;
    public static PlayerControllerInMap player;

    public bool canJump = false;
    public float playerJumpVelocity = 8f;

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerMoveState MoveState { get; set; }
    public PlayerAttackState AttackState { get; set; }
    public PlayerFightState FightState { get; set; }
    public PlayerState NullState { get; set; }
    public EnemyControllerInMap Enemy { get; set; }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
        FightState = new PlayerFightState(this, StateMachine);
        NullState = new PlayerState(this, StateMachine);
    }

    public void ChangeToNullState()
    {
        StateMachine.ChangeState(NullState);
    }
    
    public void ChangeToIdleState()
    {
        StateMachine.ChangeState(IdleState);
    }

    public IEnumerator ChangeToIdleStateAfterTime()
    {
        yield return new WaitForSeconds(0.3f);
        player.ChangeToIdleState();
    }

    void Start()
    {
        StateMachine.Initialize(IdleState);
        if (player == null)
        {
            player = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            canJump = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.currentState.FrameUpdate();
        StateMachine.currentState.AnimationUpdate();
    }
}
