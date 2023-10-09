using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<GameManager>() != null)
        {
            gameObject.SetActive(false);
        }
        transform.position = new Vector3(PlayerControllerInMap.player.transform.position.x, PlayerControllerInMap.player.transform.position.y, -10);
    }
}
