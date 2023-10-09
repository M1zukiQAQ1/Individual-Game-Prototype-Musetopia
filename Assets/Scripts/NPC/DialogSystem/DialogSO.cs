using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogPrerequisite
{
    public string questId;
    public QuestState state;
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/New Dialog")]
public class DialogSO : ScriptableObject
{
    public string id;
    public DialogBehaviorSO[] dialogBehaviors;
    [SerializeField] public DialogPrerequisite[] dialogPrerequisites;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
