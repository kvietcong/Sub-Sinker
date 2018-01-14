using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

    public float speed;
    private Rigidbody2D rb;
    private float prevXVel;
    
    GameObject camera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            gameObject.layer = 10;
        }
        rb = GetComponent<Rigidbody2D>();
        prevXVel = rb.velocity[0];
    }

    void FixedUpdate ()
    {
        // network awareness
        if (!isLocalPlayer)
            return;

        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

        rb.AddForce (movement.normalized * speed);
        BroadcastMessage("AdjustVel", Mathf.Abs(rb.velocity[0]));
    }

    // visual effects
    private void Update()
    {
        // NOT local only
        BroadcastMessage("AdjustVel", Mathf.Abs(rb.velocity[0]));

        if (prevXVel <= 0 && rb.velocity[0] > 0)
        {
            // check if sub is already in this dir
            BroadcastMessage("Flip", "right");
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.155f, 0.62f);
        }
        else if (prevXVel >= 0 && rb.velocity[0] < 0)
        {
            BroadcastMessage("Flip", "left");
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.14f, 0.62f);
        }

        prevXVel = rb.velocity[0];
    }

    public override void OnStartLocalPlayer() // local player only
    {
        // get camera
        GameObject[] cameras;
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        camera = cameras[0];

        // does not work in editor
        camera.SendMessage("SetPlayer", gameObject);
    }

}
