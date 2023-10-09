using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleEnterState
{
    SurpriseAttackByEnemy, SurpriseAttackByPlayer, NormalEncounter
}

public class GameManagerInMap : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManagerInMap instance;
    public string currentDungeon;
    public string currentEnemyName;

    //Note that the names of chests and names must not be same, otherwise issues may occur
    public List<string> enemyNameDefeated = new();
    public List<string> chestsOpened = new();
    public List<string> dropableCollected = new();

    public AudioSource music;

    private void Awake()
    {
        SceneManager.sceneLoaded += CheckIfEnteredDungeonAndUpdateSceneName;
        SceneManager.sceneLoaded += RemoveDefeatedEnemies;
        SceneManager.sceneLoaded += RemoveOpenedChests;
        SceneManager.sceneLoaded += RemoveScoreDropable;
        SceneManager.sceneLoaded += (Scene arg1, LoadSceneMode arg2) =>
        {
            if (PlayerControllerInMap.player != null)
                PlayerControllerInMap.player.ChangeToIdleState();
        };
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CheckIfEnteredDungeonAndUpdateSceneName(Scene arg1, LoadSceneMode arg2)
    {
        if (arg1.name.Contains("Dungeon"))
        {
            currentDungeon = arg1.name;
        }
    }

    //i'm an idiot
    private void RemoveScoreDropable(Scene arg1, LoadSceneMode arg2)
    {
        foreach (ScoreDropableController score in FindObjectsOfType<ScoreDropableController>())
        {
            if (dropableCollected.Contains(score.name))
                Destroy(score.gameObject);
        }
    }

    private void RemoveDefeatedEnemies(Scene arg1, LoadSceneMode arg2)
    {
        foreach (EnemyControllerInMap enemy in FindObjectsOfType<EnemyControllerInMap>())
        {
            if (enemyNameDefeated.Contains(enemy.name))
                Destroy(enemy.gameObject);
        }
    }

    private void RemoveOpenedChests(Scene arg1, LoadSceneMode arg2)
    {
        foreach (ChestController chest in FindObjectsOfType<ChestController>())
        {
            if (chestsOpened.Contains(chest.name))
                Destroy(chest.gameObject);
        }
    }

    public IEnumerator MusicHandler(AudioClip clip, float from, float to)
    {
        music.clip = clip;
        music.time = from;

        music.volume = 0;
        music.Play();

        while (music.volume < 1)
        {
            music.volume += 1f / 40f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitUntil(() => music.time >= to);

        while (music.volume > 0)
        {
            music.volume -= 1f / 40f;
            yield return new WaitForSeconds(0.05f);
        }
        music.Stop();
    }

    private IEnumerator IEStartFight(EnemyInfo[] enemies)
    {
        if (GameManagerInMainMap.instance.currentScore == null)
        {
            UIController.instance.BlinkToIndicateNullCurrentScore();
            Debug.Log("Please choose a score before entering to battle");
            yield break;
        }
        SceneManager.LoadScene("FightScene");
        yield return new WaitUntil(() => GameManager.instance != null);
        FindObjectOfType<PlayerControllerInMap>().gameObject.SetActive(false);
        GameManager.instance.StartLevel(GameManagerInMainMap.instance.currentScore, enemies);
    }

    public void EnemyEncounter(EnemyControllerInMap enemy, BattleEnterState state)
    {
        PlayerControllerInMap.player.ChangeToNullState();
        enemy.StateMachine.ChangeState(enemy.FightState);

        UIController.instance.EnableEnemyEncounterPanel(enemy);
        UIController.instance.fightStartBtn.onClick.AddListener(() => StartCoroutine(IEStartFight(enemy.info)));
        currentEnemyName = enemy.name;
    }
}
