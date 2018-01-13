using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PingShoot : NetworkBehaviour {

    public GameObject PingPrefab;
    public float force = 1000;


    // seconds per shot
    public float rateOfFire = 1f;
    float timeSinceShot;

    // Use this for initialization
    void Start()
    {
        timeSinceShot = rateOfFire;

    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        // M1
        if (Input.GetButton("Fire2") && timeSinceShot >= rateOfFire)
        {
            timeSinceShot = 0;
            Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
            CmdFirePing(mousePos);
        }

        timeSinceShot += Time.deltaTime;

    }

    // called by client, run on server
    [Command]
    void CmdFirePing(Vector2 mousePos)
    {
        var ping = Instantiate(PingPrefab, transform.position, Quaternion.identity);
        ping.GetComponent<Rigidbody2D>().AddForce(mousePos.normalized * force);

        NetworkServer.Spawn(ping);    
    }
}
