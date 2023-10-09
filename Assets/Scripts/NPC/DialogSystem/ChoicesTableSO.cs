using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestOperation
{
    START_QUEST, FINISH_QUEST
}

[System.Serializable]
public class Choice
{
    public int indexToJump;
    public string optionName;

    public bool isQuestEvent = false;
    public string questId;
    public QuestOperation questOperation;
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Choices Table", menuName = "Dialog/New Choices Table")]
public class ChoicesTableSO : DialogBehaviorSO
{
    public string header;
    public List<Choice> choices = new();
}
