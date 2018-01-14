using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingShoot : MonoBehaviour {

    public GameObject Ping;


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
        // M1
        if (Input.GetButton("Fire2") && timeSinceShot >= rateOfFire)
        {
            timeSinceShot = 0;
            FirePing();
        }

        timeSinceShot += Time.deltaTime;

    }

    void FirePing()
    {
        Ping.SetActive(true);
        Instantiate(Ping, transform.position, Quaternion.identity);
        Ping.SetActive(false);
    }
}
