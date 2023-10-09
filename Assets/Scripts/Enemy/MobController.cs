using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ToDo: Make Boss float

public class MobController : MonoBehaviour
{
    // Start is called before the first frame update
    public double health;
    public double ATK;
    public double attackCoolDown = 10f;
    public double attackDefaultInterval;
    public double speed;
    public double expGained;

    public int badJudgementsToAttak;
    public Animator animator;

    public Vector3 positionOffsetValue = new();
    public bool isBoss = false;
    public Collider2D boundBoxCollider;
    public bool isDied = false;

    private int lastGradesToAttack = 0;
    private Vector3 positionToMoveTo;
    private float timer;
    private List<PlayerControllerInFight> playersDetected = new();

    private void Awake() 
    {

    }

    void Start()
    {
        positionToMoveTo = transform.position;
    }

    public void InitializeEnemy(float health, float ATK, int expGained, bool isBoss)
    {
        this.isBoss = isBoss;
        this.health = health;
        this.ATK = ATK;
        this.expGained = expGained;
    }

    private bool JudgeIfAttack() {
        int judgeCount = GameManager.instance.GetJudgers()[(int)ROLE.DEFENDER].GetGrades()[(int)GRADE.BAD] + GameManager.instance.GetJudgers()[(int)ROLE.DEFENDER].GetGrades()[(int)GRADE.MISS];
        return (judgeCount % badJudgementsToAttak == 0) && lastGradesToAttack != judgeCount;
    }

    public void TakeDamage(double DMG)
    {
        health -= DMG;
        if(health <= 0)
        {
            isDied = true;
            return;
        }

        Vector3 randomPos =  new Vector3(UnityEngine.Random.Range(3, 4), 0, 0);
        if (boundBoxCollider.bounds.Contains(randomPos + transform.position))
        {
            positionToMoveTo = transform.position + randomPos;
            return;
        }
        positionToMoveTo = transform.position - randomPos;
//        Debug.Log(health);
    }

    private IEnumerator MoveAndAttack()
    {
        yield return new WaitUntil(() => positionToMoveTo == transform.position);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(1.3f);
        for(int i = 0; i < playersDetected.Count; i++)
        {
            playersDetected[i].TakeDamage(ATK);
        }
    }

    protected void MoveToPosition(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, (float)speed * Time.deltaTime);
    }

    private void DisplayPlayerList()
    {
        foreach(PlayerControllerInFight player in playersDetected)
        {
            Debug.Log(player);
        }
    }

    private void DealDamage()
    {
        positionToMoveTo = GameManager.instance.players[(int)ROLE.DEFENDER].transform.position + new Vector3(3.2f, 1.5f, 0);
        StartCoroutine(nameof(MoveAndAttack));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playersDetected.Add(collision.GetComponent<PlayerControllerInFight>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            for (int i = 0; i < playersDetected.Count; i++)
            {
                if (playersDetected[i].role == collision.GetComponent<PlayerControllerInFight>().role)
                {
                    playersDetected.RemoveAt(i);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (isDied)
        {
            return;
        }

        if ((JudgeIfAttack() && timer >= attackCoolDown) || timer >= attackDefaultInterval)
        {
            timer = 0;
            lastGradesToAttack = GameManager.instance.GetJudgers()[(int)ROLE.DEFENDER].GetGrades()[(int)GRADE.BAD] + GameManager.instance.GetJudgers()[(int)ROLE.DEFENDER].GetGrades()[(int)GRADE.MISS];
            DealDamage();
        }

        MoveToPosition(positionToMoveTo);

        if (positionToMoveTo != GameManager.instance.players[(int)ROLE.DEFENDER].transform.position + new Vector3(3.2f, 1.5f, 0))
        {
            StopCoroutine(nameof(MoveAndAttack));
        }

        if (positionToMoveTo.x <= transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        else if(positionToMoveTo.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
