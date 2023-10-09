using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    protected string questId;

    public void InitializeQuestStep(string questId)
    {
        Debug.Log("Quest ID initialized to " + questId);
        this.questId = questId;
    }

    public string GetQuestId()
    {
        Debug.Log("Quest Id: " + questId);
        return questId;
    }

    public abstract string GetQuestStepStats();

    protected void FinishQuestStep()
    {
        if (!isFinished) 
        {
            isFinished = true;
            Debug.Log(questId);
            GameEventManager.instance.questEvent.AdvanceQuest(questId);

            Destroy(gameObject);
        }
    }
}
