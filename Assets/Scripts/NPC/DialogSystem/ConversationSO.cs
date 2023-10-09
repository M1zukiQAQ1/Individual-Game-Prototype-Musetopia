using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialog/New Conversation")]
public class ConversationSO : DialogBehaviorSO
{
    [Header("General")]
    public int indexToJumpTo;

    [Header("Dialog")]
    public string npcName;
    [TextArea] public string[] sentence;
}
