using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject player;
    bool wasPinging;
    public GameObject PingLight;
    public float lightZOffset = -3f;
    public float firstLightIntensity = 6f;
    public float force;

    // num collisions with wall
    int collCount;

    // collisions until deactivation
    public int totalColls = 5;

    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        collCount = 0;
        FirePing();
    }

    // Update is called once per frame
    void Update()
    {
        if (collCount >= totalColls)
        {
            Destroy(this.gameObject);
        }
    }


    // todo: make new instance
    void FirePing()
    {
        transform.position = player.transform.position;
        Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        rb.AddForce(mousePos.normalized * force);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        collCount++;
        // spawn new light


        Vector3 pos = transform.position;
        pos.z = lightZOffset;

        PingLight.SetActive(true);
        GameObject light = Instantiate(PingLight, pos, transform.rotation);
        PingLight.SetActive(false);

        light.SendMessage("SetIntensity", firstLightIntensity / collCount);
    }
}