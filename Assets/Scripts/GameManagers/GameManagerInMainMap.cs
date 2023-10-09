using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerInMainMap : MonoBehaviour
{
    public static GameManagerInMainMap instance { get; private set; }

    public List<ScoreItem> scores = new();
    public ScoreItem currentScore;

    [Header("Level System")]
    public double expGained;
    public int level;
    private double expThreshold = 100f;

    [SerializeField] private Vector3 previousPos;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            GameEventManager.instance.playerEvent.onSceneChanged += PlayerEvent_onSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        PlayerControllerInMainMap playerInMainMap = FindObjectOfType<PlayerControllerInMainMap>();
        if (playerInMainMap != null)
        {
            playerInMainMap.transform.position = previousPos;
        }
    }

    private void PlayerEvent_onSceneChanged()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        PlayerControllerInMainMap playerInMainMap = FindObjectOfType<PlayerControllerInMainMap>();
        PlayerControllerInMap playerInMap = FindObjectOfType<PlayerControllerInMap>();
        if (playerInMainMap != null)
        {
            previousPos = FindObjectOfType<PlayerControllerInMainMap>().transform.position;
        }
        if(playerInMap != null)
        {
            Destroy(playerInMap.gameObject);
        }
    }
    
    public void AddExp(double expAmount)
    {
        expGained += expAmount;
        while (expGained >= expThreshold)
        {
            GameEventManager.instance.playerEvent.LevelIncremented(level);

            expGained -= expThreshold;
            level++;
            expThreshold += 5f;
        }
    }

}
