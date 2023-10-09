using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ScoreReader_New : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject note;

    private Score_New score;

    public void SetScore(Score_New s)
    {
        score = s;
    }

    public GameObject GetNotePrefab() => note;

    void Start()
    {

    }

    // Update is called once per frames
    void Update()
    {
        if(score != null)
        {
            for (int i = 0; i < score.notes_new.Count; i++)
            {
                if (score.notes_new[i].timeToPlay <= GameManager.instance.GetTime())
                {
                    if (score.notes_new[i].isLongPress)
                        Instantiate(note, transform.position, transform.rotation).GetComponent<NotesController>().SetLongPress();
                    else
                        Instantiate(note, transform.position, transform.rotation);
                    score.notes_new.RemoveAt(i);
                }

            }
        }
        
    }
}

