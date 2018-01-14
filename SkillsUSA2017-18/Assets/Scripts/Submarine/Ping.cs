using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ping : NetworkBehaviour
{
    public GameObject PingLight;
    public float lightZOffset = -3f;
    public float firstLightIntensity = 6f;

    public GameObject pinger;

    float colliderTimer;

    // num collisions with wall
    int collCount;

    // collisions until deactivation
    public int totalColls = 5;

    // does not work for some dumb reason -- method never gets called
    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        Physics2D.IgnoreCollision(pinger.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        print("ignored");
    }

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        colliderTimer = 0;
        collCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        print(colliderTimer);
        if (collCount >= totalColls)
        {
            Destroy(this.gameObject);
        }

        // enable collider after short duration (STUPID SOLUTION)
        if (colliderTimer >= 0.1f)
        {
            gameObject.GetComponent<Collider2D>().enabled = true;
        }
        else {
            colliderTimer += Time.deltaTime;
        }
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        collCount++;

        // spawn new light
        CmdSpawnLight();
    }

    [Command]
    void CmdSpawnLight()
    {
        Vector3 pos = transform.position;
        pos.z = lightZOffset;

        GameObject light = Instantiate(PingLight, pos, transform.rotation);
        light.GetComponent<Light>().intensity = firstLightIntensity / collCount;

        NetworkServer.Spawn(light);
    }
}