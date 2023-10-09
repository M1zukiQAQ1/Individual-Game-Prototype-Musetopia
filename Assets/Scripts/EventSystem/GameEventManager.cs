using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }

    public MiscEvent miscEvent;

    public PlayerEvents playerEvent;
    public InputEvents inputEvent;
    public QuestEvents questEvent;
    
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this);

        miscEvent = new();
        inputEvent = new();
        questEvent = new();
        playerEvent = new();
    }
}
