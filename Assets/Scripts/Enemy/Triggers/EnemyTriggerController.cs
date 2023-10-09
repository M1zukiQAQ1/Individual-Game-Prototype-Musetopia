using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerController : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemyControllerInMap enemy; 

    private void Start()
    {
        enemy = gameObject.GetComponentInParent<EnemyControllerInMap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            enemy.isTriggered = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.isTriggered = false;
        }
    }

}
