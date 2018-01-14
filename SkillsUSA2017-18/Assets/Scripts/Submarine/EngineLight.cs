using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EngineLight : NetworkBehaviour {
    public Light engineLight;

    public const float startRad = 10;

    [SyncVar(hook = "OnLightRadiusChange")]
    public float currentRad = startRad;
    private float newRad;
    private float scrollSpeed;

    public float maxRad = 25;
    public float minRad = 2;



    // Use this for initialization
    void Start () {
        newRad = startRad;
        engineLight.range = newRad;
        scrollSpeed = 10;
    }

    // Update is called once per frame
    void Update() {
        // network awareness
        if (!isLocalPlayer)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") < 1 && newRad >= minRad) {
            newRad -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 1 && newRad <= maxRad) {
            newRad += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }

        if (newRad < minRad) {
            newRad = minRad;
        }
        else if (newRad > maxRad) {
            newRad = maxRad;
        }

        if (newRad != currentRad) {
            // currentrad = newrad
            CmdChangeRadius(newRad);
        }
    }

    [Command]
    public void CmdChangeRadius(float radius)
    {
        currentRad = radius;
    }

    void OnLightRadiusChange(float radius)
    {
        engineLight.range = radius;
    }
}
