using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator chestAnimator;
    public ScoreItem itemContained;

    private bool playerEnteredFlag = false;
    private bool isOpenedFlag = false;

    private void Update()
    {
        if(playerEnteredFlag && Input.GetKeyDown(KeyCode.F) && !isOpenedFlag)
        {
            isOpenedFlag = true;
            chestAnimator.SetTrigger("open");
            UIController.instance.EnableNewItemReceivedPanel(itemContained);

            PlayerControllerInMap.player.ChangeToNullState();
            GameManagerInMainMap.instance.scores.Add(itemContained);

            UIController.instance.RefreshScoreSlots();
            GameManagerInMap.instance.chestsOpened.Add(name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEnteredFlag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEnteredFlag = false;
        }
    }
}
