using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

    public float speed;
    private Rigidbody2D rb;
    
    GameObject camera;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
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
