using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDungeonQuest : QuestStep
{
    private void OnEnable()
    {
        GameEventManager.instance.miscEvent.onFinishedDungeon += FinishDungeon;
    }

    private void OnDisable()
    {
        GameEventManager.instance.miscEvent.onFinishedDungeon -= FinishDungeon;
    }

    private void FinishDungeon()
    {
        Debug.Log("Finishing Quest Step!");
        FinishQuestStep();
    }

    public override string GetQuestStepStats()
    {
        return "Dungeon Not Finished";
    }
}
