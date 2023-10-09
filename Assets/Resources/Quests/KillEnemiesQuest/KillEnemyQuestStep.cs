using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyQuestStep : QuestStep
{
    private int enemiesKilled;
    private int enemiesToComplete = 2;

    private void OnEnable()
    {
        GameEventManager.instance.miscEvent.onEnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        GameEventManager.instance.miscEvent.onEnemyKilled -= EnemyKilled;
    }

    private void EnemyKilled()
    {
        Debug.Log("Enemy killed");
        enemiesKilled++;
        if(enemiesKilled >= enemiesToComplete)
        {
            Debug.Log("Quest finished");
            FinishQuestStep();
        }
    }

    public override string GetQuestStepStats()
    {
        return "Enemies killed: " + enemiesKilled + " / " + enemiesToComplete;
    }
}
