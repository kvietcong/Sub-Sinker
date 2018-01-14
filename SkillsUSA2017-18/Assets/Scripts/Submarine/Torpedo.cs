using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Torpedo : NetworkBehaviour
{
    public GameObject PingLight;
    public float lightZOffset = -3f;
    public float firstLightIntensity = 6f;

    public GameObject pinger;

    float colliderTimer;

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
    }

    // Update is called once per frame
    void Update()
    {
        // enable collider after short duration (STUPID SOLUTION)
        if (colliderTimer >= 0.15f)
        {
            gameObject.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            colliderTimer += Time.deltaTime;
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // deal damage
        // spawn explosion light + particles
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        CmdSpawnExplosion();
        Destroy(this.gameObject);
    }

    [Command]
    void CmdSpawnExplosion()
    {
        //Vector3 pos = transform.position;
        //pos.z = lightZOffset;

        //GameObject light = Instantiate(PingLight, pos, transform.rotation);
        //light.GetComponent<Light>().intensity = firstLightIntensity / collCount;

        //NetworkServer.Spawn(light);
    }
}