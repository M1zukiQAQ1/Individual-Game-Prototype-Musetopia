using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDropableController : MonoBehaviour
{
    public ScoreItem item;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!GameManagerInMainMap.instance.scores.Contains(item)){
                GameManagerInMap.instance.dropableCollected.Add(name);
                GameManagerInMainMap.instance.scores.Add(item);

                UIController.CreateNewScoreSlot(item);
            }
            Destroy(gameObject);
        }
    }
}
