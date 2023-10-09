using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerInFight : MonoBehaviour
{
    public PlayerStateMachineInFight stateMachine;
    public PlayerIdleStateInFight idleState;
    public PlayerChaseStateInFight chaseState;
    public PlayerAttackStateInFight attackState;
    public PlayerEscapeStateInFight escapeState;

    public ROLE role;
    public Animator animator;

    public int comboToAttack = 0;
    public double maxHealth;
    public double ATK;
    public double totalDamageDelt = 0;
    public float moveSpeed = 4;
    public int lastComboNum = 0;

    public Slider healthBar;

    private float indicatorOffsetValue = 1.5f;
    private double currentHealth;

    public Transform target;
    public Vector3 targetPos;

    private void Awake()
    {
        stateMachine = new();
        idleState = new(this, stateMachine);
        chaseState = new(this, stateMachine);
        attackState = new(this, stateMachine);
        escapeState = new(this, stateMachine);
    }

    void Start()
    {
        currentHealth = maxHealth;
        stateMachine.Initialize(idleState);
    }

    public void InstantiateJudgementIndicator(IndicatorController indicatorToGenerate)
    {
        IndicatorController indicatorFound = GetComponentInChildren<IndicatorController>();
        if (indicatorFound == null)
        {
            Instantiate(indicatorToGenerate, transform).transform.localPosition = new Vector3(0, indicatorOffsetValue, 0);
        }
        else if (indicatorFound != null && (indicatorToGenerate.grade == indicatorFound.grade))
        {
            indicatorFound.ResetTime();
        }
        else if (indicatorFound != null && (indicatorToGenerate.grade != indicatorFound.grade))
        {
            Destroy(indicatorFound.gameObject);
            Instantiate(indicatorToGenerate, transform).transform.localPosition = new Vector3(0, indicatorOffsetValue, 0);
        }
    }

    public double GetDamage() => ATK * GameManager.instance.GetEffs((int)role);
    public double GetHealth() => currentHealth;

    public void TakeDamage(double DMG)
    {
        if ((currentHealth - DMG) > 0 && (currentHealth - DMG) < maxHealth)
        {
            currentHealth -= DMG;
        }
        else if ((currentHealth - DMG) <= 1)
        {
            gameObject.SetActive(false);
        }

    }



    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.FrameUpdate();
        stateMachine.currentState.AnimationUpdate();
    }

    public void MoveObject(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * moveSpeed);
    }
}
