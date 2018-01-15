using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ping : NetworkBehaviour
{
    public GameObject PingLight;
    public float lightZOffset = -3f;
    public float firstLightIntensity = 6f;

    // num collisions with wall
    int collCount;

    // collisions until deactivation
    public int totalColls = 5;

    [SyncVar]
    public NetworkInstanceId spawnedBy;

    // ignore collisions on the server
    public override void OnStartClient()
    {
        GameObject obj = ClientScene.FindLocalObject(spawnedBy);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponents<Collider2D>()[0]);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponents<Collider2D>()[1]);
    }

    // Use this for initialization
    void Start()
    {
        collCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (collCount >= totalColls)
        {
            Destroy(this.gameObject);
        }        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        collCount++;

        // spawn new light
        SpawnLight();
    }

    void SpawnLight()
    {
        Vector3 pos = transform.position;
        pos.z = lightZOffset;

        GameObject light = Instantiate(PingLight, pos, transform.rotation);
        light.GetComponent<Light>().intensity = firstLightIntensity / collCount;

        NetworkServer.Spawn(light);
    }
}