using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour {

    public float maxSpeed;
    private Rigidbody2D rb;
    private float prevXVel;
    private Vector2 prevVel;
    public float wallPushbackForce;
    public RectTransform indicator;

    public Canvas[] canvases;
    
    PlayerHealth health;
    EngineLight lt;

    public GameObject model;
    public Canvas mobileCanvas;

    public bool controllerEnabled;

    public string currentDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        prevXVel = rb.velocity[0];
        health = GetComponent<PlayerHealth>();
        lt = GetComponent<EngineLight>();
        currentDir = "left";
    }

    public override void OnStartLocalPlayer() // local player only
    {
        gameObject.name = "LocalPlayer";

        // set ui active for local player only
        foreach (Canvas ui in canvases)
        {
            ui.gameObject.SetActive(true);
        }
    }

    void FixedUpdate ()
    {
        // network awareness
        if (!isLocalPlayer)
        {
            return;
        }
        if (!health.alive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            controllerEnabled = !controllerEnabled;
        }

        float moveHorizontal, moveVertical;

        if (controllerEnabled)
        {
            moveHorizontal = Input.GetAxis("C Horizontal");
            moveVertical = Input.GetAxis("C Vertical");
        }
        else
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }

        Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

        CmdAddForce(movement.normalized * maxSpeed * lt.GetLightMultiplier());

        // FIX FOR SCALE, and add buffer for edges
        indicator.anchoredPosition = new Vector2((transform.position.x / (GameManager.instance.matchSettings.mapWidth * 1.5f)) * 50f,
            (transform.position.y / (GameManager.instance.matchSettings.mapHeight * 1.5f)) * 50f);
    }

    // visual effects
    private void Update()
    {
        if (!health.alive)
        {
            return;
        }
        // NOT local only
        BroadcastMessage("AdjustVel", Mathf.Abs(rb.velocity[0]));

        if (prevXVel <= 0 && rb.velocity[0] > 0 /*&& currentDir == "left"*/)
        {
            // check if sub is already in this dir
            BroadcastMessage("Flip", "right");
        }
        else if (prevXVel >= 0 && rb.velocity[0] < 0 /*&& currentDir == "right"*/)
        {
            BroadcastMessage("Flip", "left");
        }

        prevXVel = rb.velocity[0];
        prevVel = rb.velocity;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            // damage proportional to vel
            if (isLocalPlayer)
            {
                gameObject.GetComponent<PlayerHealth>().CmdTakeDamage(prevVel.magnitude * 2, "walls");
            }
            
            CmdAddForce(-1 * prevVel * wallPushbackForce);
        }
    }

    public void FlipCollider(string dir)
    {
        currentDir = dir;
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        if (dir == "left")
        {
            // retain y position
            box.offset = new Vector2(-0.14f, box.offset.y);
        }
        else if (dir == "right")
        {
            box.offset = new Vector2(0.155f, box.offset.y);
        }
    }

    // server does physics 
    //[Command]
    public void CmdAddForce(Vector2 force)
    {
        rb.AddForce(force);
    }
}
