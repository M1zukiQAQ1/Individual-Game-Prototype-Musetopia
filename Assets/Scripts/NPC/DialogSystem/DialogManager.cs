using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public RectTransform dialogPanel;
    public TMP_Text nameText;
    public TMP_Text dialogText;

    public Button choiceButton;

    private int nextIndex = 0;
    private Coroutine displayDialogCoroutine;

    private void Start()
    {

    }

    private IEnumerator TypeDialogToDialogText(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            dialogText.text += text[i];
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator DisplayConversations(ConversationSO conversations)
    {
        Coroutine typeCoroutine;
        dialogPanel.gameObject.SetActive(true);
        foreach (string sentence in conversations.sentence)
        {
            Debug.Log("Displaying sentences");

            nameText.text = conversations.npcName;
            dialogText.text = "";
            typeCoroutine = StartCoroutine(TypeDialogToDialogText(sentence));

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

            if (sentence.Equals(dialogText.text))
            {
                continue;
            }
            StopCoroutine(typeCoroutine);
            dialogText.text = sentence;

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        }



        dialogPanel.gameObject.SetActive(false);
    }

    private IEnumerator DisplayChoiceTable(ChoicesTableSO choicesTableInfo)
    {
        int curIndex = nextIndex;
        Vector2 buttonPos = new(474, -385.11f);
        List<Button> buttonRef = new();

        foreach (Choice choice in choicesTableInfo.choices)
        {
            Button newButton = Instantiate(choiceButton);
            buttonRef.Add(newButton);

            newButton.transform.SetParent(transform);
            newButton.GetComponentInChildren<TMP_Text>().text = choice.optionName;
            newButton.onClick.AddListener(() => ChoiceButtonOnClick(choice));
            newButton.transform.localPosition = buttonPos;

            buttonPos += new Vector2(0, 60f);
        }

        yield return new WaitUntil(() => curIndex != nextIndex);
        foreach(Button btn in buttonRef)
        {
            Destroy(btn.gameObject);
        }
    }

    private void ChoiceButtonOnClick(Choice choice)
    {
        nextIndex = choice.indexToJump;
        if (choice.isQuestEvent)
        {
            switch (choice.questOperation)
            {
                case QuestOperation.START_QUEST:
                    GameEventManager.instance.questEvent.StartQuest(choice.questId);
                    break;
                case QuestOperation.FINISH_QUEST:
                    Debug.Log("----------------------Quest Finishing----------------------");
                    Debug.Log("Choice id: " + choice.questId);
                    GameEventManager.instance.questEvent.FinishQuest(choice.questId);
                    break;
                default:
                    Debug.LogError("QuestOperation Undefined!");
                    break;
            }
        }
        
        Debug.Log("Index is set to " + choice.indexToJump + " because of onclick event");
    }

    private IEnumerator DisplayDialog(DialogSO dialogInfo, int initialIndex)
    {
        Debug.Log("----------Displaying Dialog----------");
        DialogBehaviorSO currentDialog = GetDialogBehaviorByIndex(dialogInfo, initialIndex);
        nextIndex = initialIndex;

        while (nextIndex != -1)
        {
            currentDialog = GetDialogBehaviorByIndex(dialogInfo, nextIndex);

            if (currentDialog is ConversationSO)
            {
                yield return StartCoroutine(DisplayConversations(currentDialog as ConversationSO));
                nextIndex = (currentDialog as ConversationSO).indexToJumpTo;
            }
            else if (currentDialog is ChoicesTableSO)
            {
                Debug.Log("Displaying choices table, index: " + currentDialog.index);
                yield return StartCoroutine(DisplayChoiceTable(currentDialog as ChoicesTableSO));
            }

        }
        Debug.Log("----------Dialog Displayed----------");
    }

    public void DiaplayDialog(DialogSO dialogInfo)
    {
        StopAllCoroutines();
        displayDialogCoroutine = StartCoroutine(DisplayDialog(dialogInfo, 0));
    }

    private DialogBehaviorSO GetDialogBehaviorByIndex(DialogSO dialogInfo, int index)
    {
        for (int i = 0; i < dialogInfo.dialogBehaviors.Length; i++)
        {
            if (dialogInfo.dialogBehaviors[i].index.Equals(index))
            {
                return dialogInfo.dialogBehaviors[i];
            }
        }

        Debug.LogError("The index: " + index + " not found");
        return null;

    }

}
