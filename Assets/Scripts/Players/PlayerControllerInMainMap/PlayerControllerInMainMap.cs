using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInMainMap : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        GameEventManager.instance.inputEvent.KeyPressed();
    }
}
