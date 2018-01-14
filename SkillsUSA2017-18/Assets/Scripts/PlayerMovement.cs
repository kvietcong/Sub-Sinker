using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    public float illuminationRadius=10;
    public Light light;
    public float maxRad=25;
    public float minRad=2;

    private Rigidbody2D rb;
    private float currentRad;
    private float scrollSpeed;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        currentRad = illuminationRadius;
        light.range = currentRad;
        scrollSpeed = 10;
    }

    private void Update()
    {
        if(currentRad<minRad)
        {
            currentRad = minRad;
        }
        if (currentRad > maxRad)
        {
            currentRad = maxRad;
        }
        light.range = currentRad;

        if (Input.GetAxis("Mouse ScrollWheel") < 1 && currentRad >= minRad)
        {
            currentRad -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 1 && currentRad <= maxRad)
        {
            currentRad += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

        rb.AddForce (movement.normalized * speed);
    }
}
