using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotController : MonoBehaviour
{
    public ScoreItem scoreSlot;
    public Image slotImage;
    public TMP_Text difficulty; 

    public void CoverOnClick()
    {
        UIController.instance.ShowScoreInfoPanel(scoreSlot);
        UIController.instance.scoreInfoPanel.Find("Button-UseMusic").GetComponentInChildren<Button>().onClick.AddListener(UseOnClick);
//        Debug.Log(UIController.instance.scoreInfoPanel.Find("Button-UseMusic").GetComponentInChildren<Button>());
    }

    public void UseOnClick()
    {
        GameManagerInMainMap.instance.currentScore = scoreSlot;
        difficulty.text = "Selected";
        UIController.instance.scoreInfoPanel.Find("Button-UseMusic").GetComponentInChildren<Button>().onClick.RemoveListener(UseOnClick);

        UIController.instance.RefreshScoreSlots();
        UIController.instance.RefreshEnemyEncounterPanel();
    }

}
