using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

    public float speed;
    private Rigidbody2D rb;
    private float prevXVel;
    private float prevVel;
    
    GameObject camera;

    void Start()
    {
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

        CmdAddForce(movement.normalized * speed);
        BroadcastMessage("AdjustVel", Mathf.Abs(rb.velocity[0]));

        // todo: send to ui
        //print(transform.position);
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
        }
        else if (prevXVel >= 0 && rb.velocity[0] < 0)
        {
            BroadcastMessage("Flip", "left");
        }

        prevXVel = rb.velocity[0];
        prevVel = rb.velocity.magnitude;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.tag == "Map")
        {
            // damage proportional to vel
            gameObject.GetComponent<PlayerHealth>().TakeDamage(prevVel * 2);
        }
    }

    public void FlipCollider(string dir)
    {
        if (dir == "left")
        {
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.14f, 0.62f);
        }
        else if (dir == "right")
        {
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.155f, 0.62f);
        }
    }

    // server does physics 
    [Command]
    public void CmdAddForce(Vector2 force)
    {
        rb.AddForce(force);
    }

}
