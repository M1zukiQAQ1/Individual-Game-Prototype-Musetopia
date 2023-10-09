using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    public GameObject questCanStartIcon;
    public GameObject questCanFinishIcon;
    public GameObject questRequirementNotMetIcon;
    public GameObject questInProgressIcon;

    public void SetState(QuestState state, bool startPoint, bool endPoint)
    {
        questCanStartIcon.SetActive(false);
        questCanFinishIcon.SetActive(false);
        questRequirementNotMetIcon.SetActive(false);
        questInProgressIcon.SetActive(false);

        switch (state)
        {
            case QuestState.CAN_START:
                if (startPoint) { questCanStartIcon.SetActive(true); }
                break;
            case QuestState.CAN_FINISH:
                if (endPoint) { questCanFinishIcon.SetActive(true); }
                break;
            case QuestState.REQUIREMENTS_NOT_MET:
                if (startPoint) { questRequirementNotMetIcon.SetActive(true); }
                break;
            case QuestState.IN_PROGRESS:
                if (endPoint) { questInProgressIcon.SetActive(true); }
                break;
            default:
                break;
        }
    }

}
