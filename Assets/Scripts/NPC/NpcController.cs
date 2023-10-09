using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public DialogSO[] dialogsToDisplay;
    public DialogSO defaultDialog;
    
    private QuestIcon questIcon;

    private void Start()
    {
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private DialogSO GetDialogToDisplay()
    {
        Debug.Log("----------Getting Dialog To Display----------");
        bool isFullfilled = true;
        foreach(DialogSO dialog in dialogsToDisplay)
        {
            foreach(DialogPrerequisite prerequisite in dialog.dialogPrerequisites)
            {
                if (!QuestManager.instance.GetQuestById(prerequisite.questId).state.Equals(prerequisite.state))
                {
                    isFullfilled = false;
                    break;
                }
            }
            if (isFullfilled)
            {
                Debug.Log("Dialog to display is " + dialog);
                Debug.Log("----------Dialog Got----------");
                return dialog;
            }
            isFullfilled = true;
        }
        Debug.Log("----------Returning Default Dialog----------");
        return defaultDialog;
    }

    private void DisplayDialog()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("keyPressed!");
            FindObjectOfType<DialogManager>().DiaplayDialog(GetDialogToDisplay());
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEventManager.instance.inputEvent.onKeyPressed += DisplayDialog;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEventManager.instance.inputEvent.onKeyPressed -= DisplayDialog;
        }
    }
}
