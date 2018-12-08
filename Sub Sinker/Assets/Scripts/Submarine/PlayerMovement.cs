using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour {

    public float maxSpeed;
    public float minSpeed;
    private Rigidbody2D rb;
    private float prevXVel;
    private Vector2 prevVel;
    public RectTransform indicator;

    public Canvas[] canvases;
    public GameObject hearableRange;
    
    PlayerHealth health;
    EngineLight lt;

    public GameObject model;
    public GameObject bubbles;

    public string currentDir;
    //bool boost;

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
            // mobile canvas should already be active but just in case
            ui.gameObject.SetActive(true);
        }
        hearableRange.SetActive(true);
    }

    void FixedUpdate ()
    {
        // grid ui
        indicator.anchoredPosition = new Vector2((transform.position.x / (ServerManager.instance.mapWidth * 1.5f)) * 50f,
            (transform.position.y / (ServerManager.instance.mapHeight * 1.5f)) * 50f);

        // network awareness
        if (!isLocalPlayer)
        {
            return;
        }
        if (!health.alive || GameManager.instance.playerSettings.InputIsDisabled)
        {
            return;
        }

        float moveHorizontal, moveVertical;

        if (GameManager.instance.playerSettings.ControllerEnabled)
        {
            moveHorizontal = Input.GetAxis("C Horizontal");
            moveVertical = Input.GetAxis("C Vertical");
            //boost = Input.GetButton("C Boost");
        }
        else
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            //boost = Input.GetButton("Boost");
        }

        Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
        float speed = (maxSpeed - minSpeed) * lt.GetLightMultiplier() + minSpeed;
        //if(boost)
        //{
        //    // bad magic numbers :(
        //    AddForce(movement.normalized * speed * .65f);
        //    health.CmdTakeDamage(speed * Time.deltaTime * .5f, gameObject.GetComponent<PlayerInfo>().playerName,
        //            gameObject.GetComponent<PlayerInfo>().primaryColor);
        //}
        AddForce(movement.normalized * speed);
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

        var em = bubbles.GetComponent<ParticleSystem>().emission;
        em.rateOverTime = Mathf.Abs(rb.velocity[0] * 10);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            // damage proportional to vel
            if (isLocalPlayer)
            {
                gameObject.GetComponent<PlayerHealth>().CmdTakeDamage(prevVel.magnitude * 2, "the map", Color.gray);
            }
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

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force);
    }
}
