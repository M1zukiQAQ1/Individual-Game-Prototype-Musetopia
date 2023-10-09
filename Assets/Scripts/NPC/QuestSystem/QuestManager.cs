using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    private Dictionary<string, Quest> questMap;

    //Quest start requirement
    private int currentPlayerLevel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        questMap = CreateQuestMap();
    }

    public List<Quest> GetQuests()
    {
        List<Quest> quests = new();
        foreach(Quest quest in questMap.Values)
        {
            quests.Add(quest);
        }
        return quests;
    }

    private void Start()
    {
        foreach(Quest quest in questMap.Values)
        {
            GameEventManager.instance.questEvent.QuestStateChange(quest);
        }
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (arg0, arg1) => OnEnable();
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventManager.instance.questEvent.QuestStateChange(quest);
    }

    private void OnEnable()
    {
        Debug.Log("QuestManager Enabled");
        GameEventManager.instance.questEvent.onStartQuest += StartQuest;
        GameEventManager.instance.questEvent.onAdvanceQuest += AdvanceQuest;
        GameEventManager.instance.questEvent.onFinishQuest += FinishQuest;

        GameEventManager.instance.playerEvent.onLevelIncremented += PlayerLevelChange;
    }

    private void OnDisable()
    {
        if(FindObjectOfType<GameEventManager>() == null)
        {
            Debug.Log("QuestManager Disabled, unsubscribing methods from events");
            GameEventManager.instance.questEvent.onStartQuest -= StartQuest;
            GameEventManager.instance.questEvent.onAdvanceQuest -= AdvanceQuest;
            GameEventManager.instance.questEvent.onFinishQuest -= FinishQuest;

            GameEventManager.instance.playerEvent.onLevelIncremented -= PlayerLevelChange;
        }
    }

    private void PlayerLevelChange(int level)
    {
        currentPlayerLevel = level;
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        if(currentPlayerLevel < quest.info.levelRequirement)
        {
            return false;
        }

        foreach(QuestInfoSO pre in quest.info.questPrerequisites)
        {
            if(GetQuestById(pre.id).state != QuestState.FINISHED)
            {
                return false;
            }
        }

        return true;
    }

    private void Update()
    {
        foreach(Quest quest in questMap.Values)
        {
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }

    }

    private void StartQuest(string id)
    {
        GetQuestById(id).InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();
        if (quest.CurrentStepExists())
        {
            Debug.Log("The quest " + id + " advanced to new step");
            quest.InstantiateCurrentQuestStep(this.transform);
        }   
        else
        {
            Debug.Log("The quest " + id + " is set to CAN_FINISH state");
            ChangeQuestState(id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Debug.Log("Quest + " + id + " is finished, claiming reward");
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        Debug.Log("------------Claiming Rewards!-------------");
        Debug.Log(GameManagerInMainMap.instance.expGained);
        GameManagerInMainMap.instance.AddExp(quest.info.expReward);
        Debug.Log(GameManagerInMainMap.instance.expGained);
        Debug.Log("------------Rewars Claimed!-------------");

    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach(QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found");
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }

        return idToQuestMap;
    }

    public Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if(quest == null)
        {
            Debug.LogError("ID Not Found");
        }
        return quest; 
    }
}
