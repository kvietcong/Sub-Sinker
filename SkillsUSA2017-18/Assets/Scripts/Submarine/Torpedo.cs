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

    public float explosionForce = 20f;
    public float explosionRadius = 20f;

    public float hitDmg = 35f;
    public float splashDmgMax = 30f;

    public string playerName;

    // ignore collisions on the server
    public override void OnStartClient()
    {
        GameObject obj = ClientScene.FindLocalObject(spawnedBy);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponents<Collider2D>()[0]);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponents<Collider2D>()[1]);
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
                health.CmdTakeDamage(hitDmg, playerName); 
            }
        }

        // splash damage
        GameObject[] players;

        players = GameObject.FindGameObjectsWithTag("Player");

        // a_player meaning generic player, not the current player
        foreach (GameObject a_player in players)
        {
            // do not deal damage to direct hit
            if (a_player.GetComponent<Rigidbody2D>() != null)
            {
                // no splash + direct hit compounding
                if (a_player != hit.gameObject)
                {
                    var health = a_player.GetComponent<PlayerHealth>();
                    if (health != null)
                    {
                        float damage = Mathf.SmoothStep(0, splashDmgMax, (explosionRadius - Vector3.Distance(transform.position, a_player.transform.position)) / explosionRadius);
                        health.CmdTakeDamage(damage, playerName);
                    }
                }

                // add explosion force to player hit
                ExplosionForce expl = a_player.GetComponent<ExplosionForce>();
                if (isServer)
                    expl.RpcAddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        SpawnExplosion();
        Destroy(this.gameObject);
    }

    void SpawnExplosion()
    {
        //Vector3 pos = transform.position;
        //pos.z = lightZOffset;

        //GameObject exp = Instantiate(Explosion, pos, transform.rotation);

        //NetworkServer.Spawn(exp);
    }
}