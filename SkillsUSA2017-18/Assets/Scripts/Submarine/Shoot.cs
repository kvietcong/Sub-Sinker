using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Shoot : NetworkBehaviour {

    public GameObject PingPrefab;
    public GameObject TorpedoPrefab;
    public float pingForce = 1000;
    public float torpedoForce = 1500;
    public Image pingIndicator;
    public Image torpedoIndicator;
    public Image torpedoShellIndicator;

    // seconds per shot
    public float pingRateOfFire = 1f;
    float timeSincePing;

    public float torpedoRateOfFire = 2f;
    float timeSinceTorpedo;

    PlayerInventory ammo;
    PlayerHealth health;

    // Use this for initialization
    void Start()
    {
        Respawn();
        ammo = GetComponent<PlayerInventory>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (health.alive)
        {
            // left click: torpedo
            if (Input.GetButton("Fire1") && timeSinceTorpedo >= torpedoRateOfFire)
            {
                if (ammo.ChangeAmmo(-1, "torpedo"))
                {
                    torpedoIndicator.enabled = true;
                    torpedoShellIndicator.enabled = true;
                    timeSinceTorpedo = 0;
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                    Vector2 mousePos = new Vector2(Input.mousePosition.x - screenPos.x, Input.mousePosition.y - screenPos.y);
                    CmdFireTorp(mousePos, torpedoForce);
                }
                else
                {
                    print("No ammo for torpedo.");
                }
            }

            // right click: ping
            if (Input.GetButton("Fire2") && timeSincePing >= pingRateOfFire)
            {
                pingIndicator.enabled = true;
                timeSincePing = 0;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                Vector2 mousePos = new Vector2(Input.mousePosition.x - screenPos.x, Input.mousePosition.y - screenPos.y);
                CmdFirePing(mousePos, pingForce);
            }
        }

        pingIndicator.fillAmount = timeSincePing / pingRateOfFire;
        torpedoIndicator.fillAmount = timeSinceTorpedo  / torpedoRateOfFire;
        torpedoShellIndicator.fillAmount = timeSinceTorpedo / torpedoRateOfFire;

        if (timeSincePing > 1.5*pingRateOfFire)
        {
            pingIndicator.enabled = false;
        }
        if(timeSinceTorpedo > 1.5*torpedoRateOfFire)
        {
            torpedoIndicator.enabled = false;
            torpedoShellIndicator.enabled = false;
        }

        timeSincePing += Time.deltaTime;
        timeSinceTorpedo += Time.deltaTime;
    }

    // called by client, run on server
    [Command]
    void CmdFirePing(Vector2 mousePos, float firingForce)
    {
        var ping = Instantiate(PingPrefab, transform.position, Quaternion.identity);

        ping.GetComponent<Rigidbody2D>().AddForce(mousePos.normalized * firingForce);
        ping.GetComponent<Ping>().spawnedBy = netId;

        NetworkServer.Spawn(ping);    
    }

    [Command]
    void CmdFireTorp(Vector2 mousePos, float firingForce)
    {
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        var torpedo = Instantiate(TorpedoPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle + 90)));

        torpedo.GetComponent<Rigidbody2D>().AddForce(mousePos.normalized * firingForce);
        torpedo.GetComponent<Torpedo>().spawnedBy = netId;

        NetworkServer.Spawn(torpedo);
    }

    void Respawn()
    {
        timeSincePing = pingRateOfFire;
        timeSinceTorpedo = torpedoRateOfFire;
    }
}
