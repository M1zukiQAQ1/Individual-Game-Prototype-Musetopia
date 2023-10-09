using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

[Serializable]
public class Music
{
    public string name;
    public AudioClip music;
}

//ToDo: Add Health Bar to each enemy

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;

    [Header("Debug UI")]
    public RectTransform musicSelectionPanel;
    public RectTransform debugPanel;

    [Header("UI")]
    public List<TMP_Text> playerTexts;
    public TMP_InputField filename;
    public TMP_Text timeElasped;
    public Slider bossHealthBar;
    public RectTransform startPanel;
    public TMP_Text overkillText;

    [Header("Game End")]
    public RectTransform gameEndPanel;
    public RectTransform performanceInfoPanel;
    public TMP_Text warriorInfoText;
    public TMP_Text healerInfoText;
    public List<TMP_Text> judgementTexts;

    [Header("Experience Result Panel")]
    public RectTransform expPanel;
    public TMP_Text currentExpText;
    public TMP_Text resultExpText;
    public TMP_Text resultLevelText;
    public TMP_Text arrowText;
    public TMP_Text expIncreasedText;

    [Header("Indicators")]
    public List<IndicatorController> indicators = new();

    [Header("Tracks")]
    public List<ScoreReader_New> readers = new();
    public List<JudgementController> judgers = new();

    [Header("Combat")]
    public List<PlayerControllerInFight> players = new();
    public List<MobController> mobs = new();
    public List<TMP_Text> comboTexts = new();

    [Header("Misc")]
    public AudioSource music;
    public double earphoneOffsetTime;
    public List<Music> musicList;
    public BoxCollider2D boundBox;

    private string generationSceneName = "ScoreGenerator_New";
    private double[] effs = { 1, 1, 1 };
    private double offsetTime;

    private bool startedPlaying = false;
    private bool entersOverkillModeFlag = false;
    private bool uiAfterGameEndedFlag = false;
    private bool hasGameResultEvalutaed = false;

    private FullScore fs;

    private void AddEnemyKilledName()
    {
        GameManagerInMap.instance.enemyNameDefeated.Add(GameManagerInMap.instance.currentEnemyName);
    }

    private void Awake()
    {
        GameEventManager.instance.miscEvent.onEnemyKilled += AddEnemyKilledName;
    }

    public PlayerControllerInFight GetPlayerWithLowestHealth(bool isExcludeHealer)
    {
        PlayerControllerInFight temp = players[0];
        double leastHealth = players[0].GetHealth();

        foreach(PlayerControllerInFight player in players)
        {
            if(leastHealth > player.GetHealth())
            {
                if (isExcludeHealer && player.role == ROLE.HEALER)
                    continue;

                leastHealth = player.GetHealth();
                temp = player;
            }
        }
        return temp;
    }

    public double GetEffs(int index)
    {
        return effs[index];
    }

    public MobController GetMob(int index)
    {
        return mobs[index];
    }

    public List<MobController> GetMobList() => mobs;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        hasGameResultEvalutaed = false;
    }

    public List<JudgementController> GetJudgers() => judgers;

    public void SwitchToGenerationScene()
    {
        SceneManager.LoadScene(generationSceneName);
    }

    private void ReadData(ScoreItem score)
    {
        string json;
        string filepath = Application.streamingAssetsPath + "/" + score.musicVersionName + ".json";

        using (System.IO.StreamReader sr = new System.IO.StreamReader(filepath))
        {
            json = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
        }
        fs = JsonUtility.FromJson<FullScore>(json);
    }

    private void ReadData()
    {
        string json;
        string filepath = Application.streamingAssetsPath + "/" + filename.text;

        using (System.IO.StreamReader sr = new System.IO.StreamReader(filepath))
        {
            json = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
        }
        fs = JsonUtility.FromJson<FullScore>(json);
    }

    public void StartLevel(ScoreItem score, EnemyInfo[] enemies)
    {
        ReadData(score);
        music.clip = score.music;
        startPanel.gameObject.SetActive(false);

        StartCoroutine(nameof(StartGenerate));

        for (int i = 0; i < readers.Count; i++)
        {
            readers[i].SetScore(fs.scores[i]);
        }

        foreach (JudgementController judger in judgers)
        {
            judger.gameObject.SetActive(true);
        }

        foreach (PlayerControllerInFight player in players)
        {
            player.gameObject.SetActive(true);
            player.healthBar.gameObject.SetActive(true);
            player.healthBar.minValue = 0;
            player.healthBar.maxValue = (float)player.maxHealth;
            player.healthBar.value = (float)player.maxHealth;
        }

        mobs.Clear();
        StartCoroutine(GenerateMobs(enemies));

        foreach (TMP_Text comboText in comboTexts)
        {
            comboText.gameObject.SetActive(true);
        }

        for (int i = 0; i < playerTexts.Count; i++)
        {
            playerTexts[i].gameObject.SetActive(true);
            playerTexts[i].text = judgers[i].judgerIndex.ToString().Substring(0, 1) + judgers[i].judgerIndex.ToString().Substring(1).ToLower() + ": Press " + judgers[i].GetKeysString();
        }

        offsetTime = Math.Abs(readers[0].transform.position.x - judgers[0].transform.position.x) / readers[0].GetNotePrefab().GetComponent<NotesController>().noteSpeed;
        earphoneOffsetTime = score.offsetTime;
        bossHealthBar.gameObject.SetActive(true);
        bossHealthBar.minValue = 0;
        bossHealthBar.maxValue = (float)GetBoss().health;
    }

    //Use Coroutine
    private IEnumerator GenerateMobs(EnemyInfo[] enemies)
    {
        float x = 1.5f; //Where do enemies start to generate
        float y = 0f;
        foreach(var enemy in enemies)
        {
            //Randomize the time of the generation of enemies
            //Randomize bad judgememts to attack
            var curEnemy = Instantiate(enemy.enemyPrefab);
            curEnemy.GetComponent<MobController>().InitializeEnemy(enemy.health, enemy.ATK, enemy.xpReward, enemy.isBoss);
            curEnemy.transform.position = new Vector3(x, y); //Remember to change to position of each enemy
            x += UnityEngine.Random.Range(0.5f, 1.3f);
            y += UnityEngine.Random.Range(-0.5f, 0.5f);

            mobs.Add(curEnemy.GetComponent<MobController>());

            yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0.1f, 0.5f));
        }
    }

    public void StartLevel()
    {
        ReadData();
        startPanel.gameObject.SetActive(false);

        StartCoroutine(nameof(StartGenerate));

        for (int i = 0; i < readers.Count; i++)
        {
            readers[i].SetScore(fs.scores[i]);
        }

        foreach(JudgementController judger in judgers)
        {
            judger.gameObject.SetActive(true);
        }

        foreach (PlayerControllerInFight player in players)
        {
            player.gameObject.SetActive(true);
            player.healthBar.gameObject.SetActive(true);
            player.healthBar.minValue = 0;
            player.healthBar.maxValue = (float)player.maxHealth;
            player.healthBar.value = (float)player.maxHealth;
        }

        foreach (MobController mob in mobs)
        {
            mob.gameObject.SetActive(true);
        }

        foreach(TMP_Text comboText in comboTexts)
        {
            comboText.gameObject.SetActive(true);
        }

        for(int i = 0; i < playerTexts.Count; i++)
        {
            playerTexts[i].gameObject.SetActive(true);
            playerTexts[i].text = judgers[i].judgerIndex.ToString().Substring(0, 1) + judgers[i].judgerIndex.ToString().Substring(1).ToLower() + ": Press " + judgers[i].GetKeysString();
        }

        offsetTime = Math.Abs(readers[0].transform.position.x - judgers[0].transform.position.x) / readers[0].GetNotePrefab().GetComponent<NotesController>().noteSpeed;
        bossHealthBar.gameObject.SetActive(true);
        bossHealthBar.minValue = 0;
        bossHealthBar.maxValue = (float)GetBoss().health;
    }

    public void OnJudgedGrade(GRADE grade, int index)
    {
        switch (grade)
        {
            case GRADE.MISS:
                effs[index] -= 0.005;
                break;
            case GRADE.BAD:
                effs[index] -= 0.001;
                break;
            case GRADE.GREAT:
                effs[index] += 0.001;
                break;
            case GRADE.PERFECT:
                effs[index] += 0.005;
                break;
            default:
                Debug.Log("Error!");
                break;
        }
        players[index].InstantiateJudgementIndicator(indicators[(int)grade]);
    }

    public bool GetIsPlaying() => music.isPlaying;

    public double GetTime() => music.time + offsetTime + earphoneOffsetTime;

    IEnumerator StartGenerate()
    {
        timeElasped.text = "3";
        yield return new WaitForSecondsRealtime(1);
        timeElasped.text = "2";
        yield return new WaitForSecondsRealtime(1);
        timeElasped.text = "1";
        yield return new WaitForSecondsRealtime(1);
        music.Play();
        startedPlaying = true;
    }

    public MobController GetBoss()
    {
        foreach (MobController mob in mobs)
        {
            if (mob.isBoss)
            {
                return mob;
            }
        }
        return null;
    }

    public PlayerControllerInFight GetPlayerByAttackSequence()
    {
        if (players[(int)ROLE.DEFENDER].gameObject.activeSelf)
        {
            return players[(int)ROLE.DEFENDER];
        }

        else if (players[(int)ROLE.WARRIOR].gameObject.activeSelf)
        {
            return players[(int)ROLE.WARRIOR];
        }

        return players[(int)ROLE.HEALER];
    }

    private IEnumerator BlinkText()
    {
        for (int i = 0; i < 3; i++)
        {
            overkillText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            overkillText.color = Color.black;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void DebugKillAll()
    {
        foreach(MobController mob in mobs)
        {
            mob.TakeDamage(mob.health + 10);
        }
    }
    public void DebugEndBattle()
    {
        music.time = music.clip.length - 5f;
    }

    public void DebugOpenMusicSelectionPanel()
    {
        debugPanel.gameObject.SetActive(false);
        musicSelectionPanel.gameObject.SetActive(true);
    }

    public void EnterOverkillMode()
    {
        overkillText.gameObject.SetActive(true);
        StartCoroutine(nameof(BlinkText));
        bossHealthBar.value = 0;
    }

    public void ReturnToMainMap()
    {
        PlayerControllerInMap.player.gameObject.SetActive(true);
        SceneManager.LoadScene(GameManagerInMap.instance.currentDungeon);
    }

    private void UpdateUIAfterGameEnd()
    {
        gameEndPanel.gameObject.SetActive(true);
        for (int i = 0; i < judgementTexts.Count; i++)
        {
            string str = ((ROLE)i).ToString() + ": ";
            for (int j = 0; j < judgers[i].GetGrades().Length; j++)
            {
                str += ((GRADE)j).ToString().Substring(0, 1) + ((GRADE)j).ToString().Substring(1).ToLower() + ": " + judgers[i].GetGrades()[j].ToString() + "  ";
            }
            judgementTexts[i].text = str;
        }
        bossHealthBar.gameObject.SetActive(false);

        warriorInfoText.text = "Total Damage: " + players[(int)ROLE.WARRIOR].totalDamageDelt.ToString("0");
        healerInfoText.text = "Total Heal: " + (players[(int)ROLE.HEALER].totalDamageDelt * -1).ToString("0");
    }

    public MobController GetMobByAttackSequence()
    {
        foreach(MobController mob in mobs)
        {
            if (mob.isBoss && !mob.isDied)
            {
                return mob;
            }
        }

        foreach (MobController mob in mobs)
        {
            if (!mob.isDied)
            {
                return mob;
            }
        }

        return mobs[UnityEngine.Random.Range(0, mobs.Count)];
    }

    private IEnumerator DisplayExpPanel(double expIncreased)
    {
        expPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        currentExpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        arrowText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        expIncreasedText.text = "+" + expIncreased.ToString("0");
        expIncreasedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        resultLevelText.text = GameManagerInMainMap.instance.level.ToString("0");
        resultLevelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        resultExpText.text = GameManagerInMainMap.instance.expGained.ToString("0");
        resultExpText.gameObject.SetActive(true);
    }

    public void UpdateAfterSelectingNextAfterGameEnded()
    {
        if (hasGameResultEvalutaed)
        {
            return;
        }
        hasGameResultEvalutaed = true;
        performanceInfoPanel.gameObject.SetActive(false);
        double exp = 0.0d;
        currentExpText.text = GameManagerInMainMap.instance.expGained.ToString("0");
        foreach(MobController mob in mobs)
        {
            exp += mob.expGained;
        }
        GameManagerInMainMap.instance.AddExp(exp);
        StartCoroutine(nameof(DisplayExpPanel), exp);
    }

    private void UpdateAfterGameEnded()
    {
        foreach (PlayerControllerInFight player in players)
        {
            player.gameObject.SetActive(false);
            player.healthBar.gameObject.SetActive(false);
        }

        foreach (MobController mob in mobs)
        {
            mob.gameObject.SetActive(false);
        }

        foreach (TMP_Text comboText in comboTexts)
        {
            comboText.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if(music.time != 0)
        {
            timeElasped.text = GetTime().ToString("0.00") + " " + music.time.ToString("0.00");
        }

        for (int i = 0; i < comboTexts.Count; i++)
        {
            comboTexts[i].text = judgers[i].GetComboCounter().ToString("0");
        }

        if(GetBoss() != null)
        {
            bossHealthBar.value = (float)GetBoss().health;
        }

        for(int i = 0; i < players.Count; i++)
        {
            players[i].healthBar.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(players[i].transform.position - new Vector3(0, 1.1f, 0));
            players[i].healthBar.value = (float)players[i].GetHealth();
        }

        if (overkillText.gameObject.activeSelf)
        {
            overkillText.text = "Overkill! +" + Math.Abs(GetBoss().health).ToString("0");
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if (GetBoss().isDied && !entersOverkillModeFlag)
        {
            Debug.Log("misc events invoked");
            GameEventManager.instance.miscEvent.EnemyKilled();
            entersOverkillModeFlag = true;
            EnterOverkillMode();
        }

        if(startedPlaying && !music.isPlaying && !uiAfterGameEndedFlag)
        {
            UpdateAfterGameEnded();
            UpdateUIAfterGameEnd();
            uiAfterGameEndedFlag = true;
        }

    }
}
