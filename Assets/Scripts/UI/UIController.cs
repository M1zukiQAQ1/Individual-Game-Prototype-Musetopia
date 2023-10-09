using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Button startFightBtn;

    [Header("New Item Received Panel")]
    public RectTransform newItemReceivedPanel;
    public TMP_Text hintText;
    public TMP_Text scoreNameText;

    [Header("Score Info Panel")]
    public RectTransform scoreInfoPanel;
    public TMP_Text scoreName;
    public Image scoreCover;

    [Header("Backpack Panel")]
    public Transform backpackObject;
    public RectTransform backpackSlotPanel;
    public Button openBackpackBtn;
    public GameObject slotGrid;
    public SlotController slot;

    [Header("Enemy Encounter Panel")]
    public RectTransform enemyEncounterPanel;
    public Image enemyImage;
    public TMP_Text levelText;
    public TMP_Text enemyNameText;
    public TMP_Text scoreDifficulty;
    public Button fightStartBtn;

    private Coroutine enableItemReceivedPanelCoroutine;
    private Coroutine musicHandlerCoroutine;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        
    }

    public void EnableEnemyEncounterPanel(EnemyControllerInMap enemy)
    {
        enemyEncounterPanel.gameObject.SetActive(true);
        enemyImage.sprite = enemy.GetComponentInChildren<SpriteRenderer>().sprite;
        enemyImage.SetNativeSize();
        levelText.text = "Enemy Highest Level: " + enemy.GetHighestEnemyLevel();

        if (GameManagerInMainMap.instance.currentScore != null)
            scoreDifficulty.text = "Score Difficulty: " + GameManagerInMainMap.instance.currentScore.difficulty.ToString();
        else
            scoreDifficulty.text = "Select a score first";

//        enemyNameText.text = enemyName;
    }

    private void RefreshEnemyEncounterPanel(EnemyControllerInMap enemy)
    {
        enemyImage.sprite = enemy.GetComponentInChildren<SpriteRenderer>().sprite;
        enemyImage.SetNativeSize();
    }

    public void BlinkToIndicateNullCurrentScore()
    {
        StartCoroutine(nameof(BlinkText));
    }
    private IEnumerator BlinkText()
    {
        for(int i = 0; i < 3; i++)
        {
            scoreDifficulty.color = Color.red;
            yield return new WaitForSeconds(0.07f);
            scoreDifficulty.color = Color.black;
            yield return new WaitForSeconds(0.07f);
        }
    }

    //Todo: move to "GamaManagerInMap" class
    public void EnableNewItemReceivedPanel(ScoreItem score)
    {
        enableItemReceivedPanelCoroutine = StartCoroutine(EnableNewItemReceivedPanel("You discovered the score named \r\n", score));
    }

    //ToDo: move to "GameManagerInMap" class
    //This is bad, but i cant any solution better
    private IEnumerator CheckIfMouseButtonDown(string textToShow, ScoreItem score)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        if (!GameManagerInMap.instance.music.isPlaying)
        {
            StartCoroutine(PlayerControllerInMap.player.ChangeToIdleStateAfterTime());
            newItemReceivedPanel.gameObject.SetActive(false);
            yield break;
        }

        StopCoroutine(musicHandlerCoroutine);
        GameManagerInMap.instance.music.Stop();
        hintText.text = textToShow;
        scoreNameText.text = score.musicName;
        StopCoroutine(enableItemReceivedPanelCoroutine);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        StartCoroutine(PlayerControllerInMap.player.ChangeToIdleStateAfterTime());
        newItemReceivedPanel.gameObject.SetActive(false);
    }

    //ToDo: move to "GameManagerInMap" class
    private IEnumerator EnableNewItemReceivedPanel(string textToShow, ScoreItem score)
    {
        hintText.text = "";
        scoreNameText.text = "";

        yield return new WaitForSeconds(1.5f);

        musicHandlerCoroutine = StartCoroutine(GameManagerInMap.instance.MusicHandler(score.music, score.previewFrom, score.previewTo));
        newItemReceivedPanel.gameObject.SetActive(true);
        newItemReceivedPanel.position = Camera.main.WorldToScreenPoint(PlayerControllerInMap.player.transform.position + new Vector3(0, 2f));

        StartCoroutine(CheckIfMouseButtonDown(textToShow, score));
        for (int i = 0; i < textToShow.Length; i++)
        {
            hintText.text += textToShow[i];
            yield return new WaitForSeconds(0.1f);
        }

        for(int i = 0; i < score.musicName.Length; i++)
        {
            scoreNameText.text += score.musicName[i];
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RefreshEnemyEncounterPanel()
    {
        if (GameManagerInMainMap.instance.currentScore != null)
            scoreDifficulty.text = "Score Difficulty: " + GameManagerInMainMap.instance.currentScore.difficulty.ToString();
        else
            scoreDifficulty.text = "Score Difficulty: Please select a score";
    }

    public void ShowScoreInfoPanel(ScoreItem item)
    {
        scoreInfoPanel.gameObject.SetActive(true);
        scoreName.text = item.musicName;
        scoreCover.sprite = item.cover;
    }

    public void EnableBackpackPanel()
    {
        PlayerControllerInMap.player.ChangeToNullState();
        backpackObject.gameObject.SetActive(true);
    }

    public void DisableBackpackPanel()
    {
        backpackObject.gameObject.SetActive(false);
        PlayerControllerInMap.player.ChangeToIdleState();
    }

    public void RefreshScoreSlots()
    {
        foreach (SlotController slot in backpackSlotPanel.GetComponentsInChildren<SlotController>())
            Destroy(slot.gameObject);
        foreach(ScoreItem item in GameManagerInMainMap.instance.scores)
        {
            CreateNewScoreSlot(item);
        }
    }

    private void Update()
    {
        if(backpackSlotPanel != null && backpackSlotPanel.GetComponentsInChildren<SlotController>().Length == 0)
        {
            RefreshScoreSlots();
        }
    }

    public static void CreateNewScoreSlot(ScoreItem item)
    {
        SlotController newItem = Instantiate(instance.slot, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);

        newItem.scoreSlot = item;
        newItem.slotImage.sprite = item.cover;
        newItem.difficulty.text = item.difficulty.ToString();

        if (GameManagerInMainMap.instance.currentScore != null && item.Equals(GameManagerInMainMap.instance.currentScore))
        {
            newItem.difficulty.text = "Selected";
        }
    }
}
