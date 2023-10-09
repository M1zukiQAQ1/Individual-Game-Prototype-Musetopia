using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControllerInMainMap : MonoBehaviour
{

    public static UIControllerInMainMap instance; 

    [Header("Diaplay Quest System")]
    public Button questBtn;
    public RectTransform questInfoPanel;
    public RectTransform questScrollContent;
    public GameObject questPanel;
    public TMP_Text questHeaderText;
    public TMP_Text questInfoText;

    private List<Button> questBtns = new();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += (arg0, arg1) =>
        {
            GameEventManager.instance.questEvent.onStartQuest += RefreshQuestInfoContent;
            GameEventManager.instance.questEvent.onAdvanceQuest += RefreshQuestInfoContent;
        };
    }

    private string GetQuestStats(string id)
    {
        Debug.Log("----------Getting Quest Stats----------");
        foreach (QuestStep qs in QuestManager.instance.GetComponentsInChildren<QuestStep>())
        {
            Debug.Log(qs);
            Debug.Log(qs.GetQuestId());
            Debug.Log(id);
            if (qs.GetQuestId().Equals(id))
            {
                Debug.Log("----------Quest Stats Got----------");
                return qs.GetQuestStepStats();
            }
                
        }
        Debug.Log("Unable to find quest of the id " + id);
        return null;
    }

    public void RefreshQuestInfoContent(string arg0)
    {
        Debug.Log("----------Refreshing QuestInfo----------");
            
        Button questBtn;

        questHeaderText.text = "";
        questInfoText.text = "";

        foreach(Button btn in questBtns)
        {
            if(btn == null)
            {
                continue;
            }

            Destroy(btn.gameObject);
        }
        questBtns.Clear();

        foreach(Quest quest in QuestManager.instance.GetQuests())
        {
            if(quest.state.Equals(QuestState.IN_PROGRESS))
            {
                questBtn = Instantiate(this.questBtn, questScrollContent);

 //               Debug.Log("Quest Btn -> " + questBtn);

                questBtn.onClick.AddListener(() => QuestButtonOnClick(GetQuestStats(quest.info.id), quest.info.displayName));
                questBtn.GetComponentInChildren<TMP_Text>().text = quest.info.displayName;
                questBtns.Add(questBtn);
            }

            else if (quest.state.Equals(QuestState.CAN_FINISH))
            {
                questBtn = Instantiate(this.questBtn, questScrollContent);
                questBtn.onClick.AddListener(() => QuestButtonOnClick("Quest Completed!", quest.info.displayName));
                questBtn.GetComponentInChildren<TMP_Text>().text = quest.info.displayName;
                questBtns.Add(questBtn);
            }

        }
        Debug.Log("----------QuestInfo Refreshed----------");
    }

    public void CloseQuestPanel()
    {
        questHeaderText.text = "";
        questInfoText.text = "";
        questPanel.gameObject.SetActive(false);
    }

    private void QuestButtonOnClick(string questStatToDisplay, string questHeaderToDisplay)
    {
        questHeaderText.text = questHeaderToDisplay;
        questInfoText.text = questStatToDisplay;
    }
}
