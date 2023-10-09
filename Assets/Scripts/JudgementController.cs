using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GRADE
{
    MISS, BAD, GREAT, PERFECT
}

public class JudgementController : MonoBehaviour
{
    // Start is called before the first frame update
    private float goodRange = 0.3f;
    private float badRange = 0.7f;
    private float missRange = 1.5f;
    private int comboCounter = 0;

    private GRADE gradeJudged;
    private int[] gradeArr = { 0, 0, 0, 0 };
    
    public ParticleSystem destroyParticle;
    public ROLE judgerIndex;

    public List<KeyCode> keys;

    private Queue<NotesController> notes = new();

    void Start()
    {
        
    }

    private GRADE JudgeRange(float distance)
    {
        if(distance > missRange)
        {
            comboCounter = 0;
            return GRADE.MISS;
        }
        else if(distance > badRange && distance <= missRange)
        {
            comboCounter++;
            return GRADE.BAD;
        }
        else if(distance > goodRange && distance <= badRange)
        {
            comboCounter++;
            return GRADE.GREAT;
        }
        else
        {
            comboCounter++;
            return GRADE.PERFECT;
        }
    }

    private bool IsKeyPressed()
    {
        for(int i = 0; i < keys.Count; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                return true;
            }
        }
        return false;
    }

    public string GetKeysString()
    {
        string str = "";
        for (int i = 0; i < keys.Count; i++)
        {
            str += keys[i].ToString() + " ";
        }

        return str;
    }

    // Update is called once per frame
    void Update()
    {

        if (notes.Count != 0 && notes.Count != 0 && ((IsKeyPressed() && !notes.Peek().GetIsLongPress()) || (IsKeyPressed() && notes.Peek().GetIsLongPress())))
        {
            gradeJudged = JudgeRange((notes.Peek().transform.position - transform.position).magnitude);
            gradeArr[(int)gradeJudged]++;
            GameManager.instance.OnJudgedGrade(gradeJudged, (int)judgerIndex);
            Instantiate(destroyParticle, notes.Peek().transform.position, destroyParticle.transform.rotation);
            Destroy(notes.Dequeue().gameObject);
        }

        if(notes.Count != 0 && (notes.Peek().transform.position - transform.position).magnitude > missRange && notes.Peek().transform.position.x < transform.position.x)
        {
            comboCounter = 0;
            gradeArr[(int)GRADE.MISS]++;
            GameManager.instance.OnJudgedGrade(GRADE.MISS, (int)judgerIndex);
            Destroy(notes.Dequeue().gameObject);
        }
    }

    public int[] GetGrades() => gradeArr;

    public int GetComboCounter() => comboCounter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "note")
        {
            notes.Enqueue(collision.GetComponent<NotesController>());
        }
    }


}
