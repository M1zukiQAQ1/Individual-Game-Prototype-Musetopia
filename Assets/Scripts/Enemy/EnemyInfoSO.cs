using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo{
    public int level;
    public bool isBoss;

    public float health;
    public float ATK;

    public int xpReward;
    public int goldReward;

    public GameObject enemyPrefab;
}
