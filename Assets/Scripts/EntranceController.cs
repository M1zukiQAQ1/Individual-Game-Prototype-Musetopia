using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EntranceController : MonoBehaviour
{
    public string sceneNameToLoad;

    private void EnterScene()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameEventManager.instance.inputEvent.onKeyPressed -= EnterScene;

            if (sceneNameToLoad.Equals("MainMap") && SceneManager.GetActiveScene().name.Contains("Dungeon"))
            {
//                Debug.Log("Invoking finish dungeon event");
                GameEventManager.instance.miscEvent.FinishDungeon();
            }

            GameEventManager.instance.playerEvent.SceneChanged();
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
 //       Debug.Log("Triggered");
        if (collision.CompareTag("Player"))
        {
            GameEventManager.instance.inputEvent.onKeyPressed += EnterScene;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEventManager.instance.inputEvent.onKeyPressed -= EnterScene;
        }
    }
}
