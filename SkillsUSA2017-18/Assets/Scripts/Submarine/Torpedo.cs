using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Torpedo : NetworkBehaviour
{
    //public GameObject PingLight;
    //public float lightZOffset = -3f;
    //public float firstLightIntensity = 6f;

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
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // deal damage
        // spawn explosion light + particles
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        GameObject hit = coll.gameObject;

        // direct hit
        if (hit.tag == "Player")
        {
            var health = hit.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(35); 
            }
        }

        // splash damage
        GameObject[] enemies;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // needs rigidbody for explosion to work
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Rigidbody>() != null)
            {
                // add an explosion function to the enemies that takes position as argument
                enemy.SendMessage("AddPineappleExplosion", transform.position);
            }
        }

        SpawnExplosion();
        Destroy(this.gameObject);
    }

    void SpawnExplosion()
    {
        //Vector3 pos = transform.position;
        //pos.z = lightZOffset;

        //GameObject light = Instantiate(PingLight, pos, transform.rotation);
        //light.GetComponent<Light>().intensity = firstLightIntensity / collCount;

        //NetworkServer.Spawn(light);
    }
}