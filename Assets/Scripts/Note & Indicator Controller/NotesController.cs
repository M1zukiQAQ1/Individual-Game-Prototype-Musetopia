using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesController : MonoBehaviour
{
    // Start is called before the first frame update
    public float noteSpeed;
    public bool debugMode = false;

    private bool isLongPressed = false;

    void Start()
    {
        noteSpeed *= -1;
        if (debugMode)
        {
            Debug.Log(GameManager.instance.GetTime());
        }
    }

    public bool GetIsLongPress() => isLongPressed;

    public void SetLongPress() => isLongPressed = true;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(noteSpeed * Time.deltaTime, 0, 0);
        if(debugMode && transform.position.x == GameManager.instance.GetJudgers()[0].transform.position.x)
        {
            Debug.Log(GameManager.instance.GetTime());
        }
    }

}
