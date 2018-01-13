using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EngineLight : NetworkBehaviour {
    public Light engineLight;

    [SyncVar(hook = "OnLightRadiusChange")]
    public float currentRad;
    private float scrollSpeed;

    public float maxRad = 25;
    public float minRad = 2;

    public float startRad = 10;

    // Use this for initialization
    void Start () {
        currentRad = startRad;
        engineLight.range = currentRad;
        scrollSpeed = 10;
    }
	
	// Update is called once per frame
	void Update () {
        // network awareness
        if (!isLocalPlayer)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") < 1 && currentRad >= minRad)
        {
            currentRad -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 1 && currentRad <= maxRad)
        {
            currentRad += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }

        if (currentRad < minRad)
            { currentRad = minRad; }
        else if (currentRad > maxRad)
            { currentRad = maxRad; }
    }

    void OnLightRadiusChange(float radius)
    {
        engineLight.range = radius;
        print(radius);
    }
}
