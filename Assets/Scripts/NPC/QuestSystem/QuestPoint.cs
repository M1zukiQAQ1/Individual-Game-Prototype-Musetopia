using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    public bool startPoint;
    public bool endPoint;

    private string questId;
    private QuestState currentQuestState;
    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void Start()
    {
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        GameEventManager.instance.questEvent.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventManager.instance.questEvent.onQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, endPoint);
        }
    }

    private void KeyPressed()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventManager.instance.questEvent.StartQuest(questId);
            }
            else if(currentQuestState.Equals(QuestState.CAN_FINISH) && endPoint)
            {
                GameEventManager.instance.questEvent.FinishQuest(questId);
            }
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameEventManager.instance.inputEvent.onKeyPressed += KeyPressed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameEventManager.instance.inputEvent.onKeyPressed -= KeyPressed;
    }

}
