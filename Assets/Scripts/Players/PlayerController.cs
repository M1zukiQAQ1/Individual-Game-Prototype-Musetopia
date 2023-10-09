using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public enum ROLE
{
    HEALER, WARRIOR, DEFENDER, NONE
}

//Obosolete
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public ROLE role;

    public bool isAutoAttack = false;
    public int comboToAttack = 0;

    public double ATK;
    public double totalDamageDelt = 0;
    public double maxHealth;
    public double speed;

    public Slider healthBar;

    private float indicatorOffsetValue = 1.5f;
    private bool hasNotAttacked = true;
    private double timeToAttack = 0;
    private int lastComboNum = 0;
    private double health;

    private Vector3 positionOffsetValue = new(0, 1f, 0);
    protected Vector3 positionToMoveTo = new();

    void Start()
    {
        health = maxHealth;
        positionToMoveTo = transform.position;
    }

    public Vector3 GetPositionToMoveTo() => transform.position + positionOffsetValue;

    public void InstantiateJudgementIndicator(IndicatorController indicatorToGenerate)
    {
        IndicatorController indicatorFound = GetComponentInChildren<IndicatorController>();
        if (indicatorFound == null)
        {
            Instantiate(indicatorToGenerate, transform).transform.localPosition = new Vector3(0, indicatorOffsetValue, 0);
        }
        else if(indicatorFound != null && (indicatorToGenerate.grade == indicatorFound.grade))
        {
            indicatorFound.ResetTime();
        }
        else if(indicatorFound != null && (indicatorToGenerate.grade != indicatorFound.grade))
        {
            Destroy(indicatorFound.gameObject);
            Instantiate(indicatorToGenerate, transform).transform.localPosition = new Vector3(0, indicatorOffsetValue, 0);
        }
    }

    protected void MoveToPosition(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, (float)speed * Time.deltaTime);
    }

    public double GetHealth() => health;

    public void TakeDamage(double DMG)
    {
        if((health - DMG) > 0 && (health - DMG) < maxHealth)
        {
            health -= DMG;
        }
        else if((health - DMG) <= 1)
        {
            gameObject.SetActive(false);
        }

    }

    protected double GetDamage() 
    {
        return ATK * GameManager.instance.GetEffs((int)role);
    }

    protected virtual void DealDamage()
    {
        if(animator != null)
        {
            animator.SetTrigger("attack");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Change Animation State
        if (positionToMoveTo.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (!isAutoAttack && GameManager.instance.GetJudgers()[(int)role].GetComboCounter() % comboToAttack == 0 && lastComboNum != GameManager.instance.GetJudgers()[(int)role].GetComboCounter())
        {
            DealDamage();
            lastComboNum = GameManager.instance.GetJudgers()[(int)role].GetComboCounter();
        }

        if (isAutoAttack && GameManager.instance.GetIsPlaying())
        {
            if (hasNotAttacked)
            {
                timeToAttack = Random.Range((float)GameManager.instance.GetTime() + 5, (float)GameManager.instance.GetTime() + 15);
                hasNotAttacked = false;
            }
            if(!hasNotAttacked && timeToAttack <= GameManager.instance.GetTime())
            {
                DealDamage();
                hasNotAttacked = true;
            }
        }

        animator.SetBool("isMoving", transform.position != positionToMoveTo);

    }
}
