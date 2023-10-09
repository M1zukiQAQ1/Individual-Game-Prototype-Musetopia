using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    REQUIREMENTS_NOT_MET, 
    CAN_START,
    IN_PROGRESS,
    CAN_FINISH,
    FINISHED
}

public class Quest
{
    public QuestInfoSO info;

    public QuestState state;
    private GameObject questStepPrefab;

    private int currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        info = questInfo;
        state = QuestState.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return currentQuestStepIndex < info.questStepPrefabs.Length;
    }

    public void InstantiateCurrentQuestStep(Transform parentTransForm)
    {
        questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab != null)
        {
            Object.Instantiate(questStepPrefab, parentTransForm).GetComponent<QuestStep>().InitializeQuestStep(info.id);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.Log("Out of range");
        }
        return questStepPrefab;
    }

}
