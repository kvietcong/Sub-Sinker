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
    public Text torpedoBar;

    // seconds per shot
    public float pingRateOfFire = 1f;
    float timeSincePing;

    public float torpedoRateOfFire = 2f;
    float timeSinceTorpedo;
    string[] stages = new string[] { " [] ", " []  []  [] ", " []  []  []  []  [] ", " []  []  []  []  []  []  [] ", "Torpedo Ready", "No Torpedos Left"};
    float torpedoPercentage;

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

        torpedoPercentage = timeSinceTorpedo / torpedoRateOfFire * 100;
        pingIndicator.fillAmount = timeSincePing / pingRateOfFire;

        if (timeSincePing > 1.5 * pingRateOfFire)
        {
            pingIndicator.enabled = false;
        }

        if(torpedoPercentage>100)
        {
            torpedoBar.text = stages[4];
        }
        if (torpedoPercentage < 95)
        {
            torpedoBar.text = stages[3];
        }
        if (torpedoPercentage < 65)
        {
            torpedoBar.text = stages[2];
        }
        if (torpedoPercentage < 45)
        {
            torpedoBar.text = stages[1];
        }
        if (torpedoPercentage < 25)
        {
            torpedoBar.text = stages[0];
        }
        if(ammo.currentAmmo[0]<=0)
        {
            torpedoBar.text = stages[5];
        }

        timeSincePing += Time.deltaTime;
        timeSinceTorpedo += Time.deltaTime;

        if (!health.alive || GameManager.instance.playerSettings.InputIsDisabled)
        {
            return;
        }

        bool shooting, pinging;
        if (GameManager.instance.playerSettings.ControllerEnabled)
        {
            shooting = Input.GetAxisRaw("C Torpedo") >= 0.5f;
            pinging = Input.GetAxisRaw("C Ping") >= 0.5f;
        }
        else
        {
            shooting = Input.GetButton("Torpedo");
            pinging = Input.GetButton("Ping");
        }

        if (shooting && timeSinceTorpedo >= torpedoRateOfFire)
        {
            if (ammo.ChangeAmmo(-1, "torpedo"))
            {
                timeSinceTorpedo = 0;
                Vector2 direction = GetDirection();
                CmdFireTorp(direction, torpedoForce);
            }
            else
            {
                print("No ammo for torpedo.");
            }
        }

        if (pinging && timeSincePing >= pingRateOfFire)
        {
            pingIndicator.enabled = true;
            timeSincePing = 0;
            Vector2 direction = GetDirection();
            CmdFirePing(direction, pingForce);
        }
    }

    // called by client, run on server
    [Command]
    void CmdFirePing(Vector2 direction, float firingForce)
    {
        var ping = Instantiate(PingPrefab, transform.position, Quaternion.identity);

        ping.GetComponent<Rigidbody2D>().AddForce(direction.normalized * firingForce);
        ping.GetComponent<Ping>().spawnedBy = netId;

        NetworkServer.Spawn(ping);    
    }

    [Command]
    void CmdFireTorp(Vector2 direction, float firingForce)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var torpedo = Instantiate(TorpedoPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle + 90)));

        torpedo.GetComponent<Rigidbody2D>().AddForce(direction.normalized * firingForce);
        torpedo.GetComponent<Torpedo>().spawnedBy = netId;
        torpedo.GetComponent<Torpedo>().srcPos = transform.position;

        NetworkServer.Spawn(torpedo);
    }

    void Respawn()
    {
        timeSincePing = pingRateOfFire;
        timeSinceTorpedo = torpedoRateOfFire;
    }

    Vector2 GetDirection()
    {
        if (GameManager.instance.playerSettings.ControllerEnabled)
        {
            Vector2 direction = new Vector2(Input.GetAxis("C HorizontalShoot"), Input.GetAxis("C VerticalShoot"));
            if (direction.magnitude == 0)
            {
                if (gameObject.GetComponent<PlayerMovement>().currentDir == "right")
                {
                    direction.x = 1;
                }
                else
                {
                    direction.x = -1;
                }
            }
            return direction;
        }
        else
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 direction = new Vector2(Input.mousePosition.x - screenPos.x, Input.mousePosition.y - screenPos.y);
            return direction;
        }
    }
}
