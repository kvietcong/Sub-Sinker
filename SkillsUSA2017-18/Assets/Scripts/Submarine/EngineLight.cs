using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EngineLight : NetworkBehaviour {
    public Light engineLight;

    public const float startRad = 10;

    [SyncVar(hook = "OnLightRadiusChange")]
    public float currentRad;
    private float newRad;
    private float scrollSpeed;

    public float maxRad = 25;
    public float minRad = 2;
    private float startIntensity;

    public RectTransform circle;
    public GameObject model;
    Text debugText;

    GameObject localPlayer;
    PlayerHealth health;
    EngineLight lt;

    public bool controllerEnabled;

    // Use this for initialization
    void Start () {
        localPlayer = GameObject.Find("LocalPlayer");
        scrollSpeed = 10;
        startIntensity = engineLight.intensity;
        health = GetComponent<PlayerHealth>();
        Spawn();
    }

    // Update is called once per frame
    void Update() {
        if (!health.alive)
        {
            // vvvv   maybe use this instead   vvvv
            //engineLight.intensity = 0;
            if (currentRad != 0 && isLocalPlayer)
            {
                
                CmdChangeRadius(0); // light off
            }
            return;
        }

        // i eyeballed this value....
        if (Vector3.Distance(transform.position, localPlayer.transform.position) > currentRad * currentRad * 0.08f)
        {
            // hide light when not within distance of localplayer
            engineLight.intensity = 0;
        }
        else
        {
            engineLight.intensity = startIntensity;
        }

        // network awareness
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.F3))
        {
            controllerEnabled = !controllerEnabled;
        }

        if (controllerEnabled)
        {
            newRad += (Input.GetAxisRaw("C EngineUp") - Input.GetAxisRaw("C EngineDown")) * 0.2f; // slow it
            if (newRad == currentRad)
            {
                newRad = Mathf.Round(currentRad);
            }
        }
        else
        {
            if (Input.GetAxis("EngineLight") < 1 && newRad >= minRad)
            {
                newRad -= Input.GetAxis("EngineLight") * scrollSpeed;
            }
            else if (Input.GetAxis("EngineLight") > 1 && newRad <= maxRad)
            {
                newRad += Input.GetAxis("EngineLight") * scrollSpeed;
            }
        }

        if (newRad < minRad) {
            newRad = minRad;
        }
        else if (newRad > maxRad) {
            newRad = maxRad;
        }

        if (newRad != currentRad) {
            // change the light instantaneously, so you dont have to wait for the server
            engineLight.range = newRad;
            engineLight.spotAngle = newRad * 7;
            circle.sizeDelta = new Vector2(newRad * newRad * 2.8f, newRad * newRad * 2.8f);
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
        currentRad = radius;
        engineLight.range = radius;
        engineLight.spotAngle = radius * 7;
        circle.sizeDelta = new Vector2(radius * radius * 2.8f, radius * radius * 2.8f);
    }

    public float GetLightMultiplier()
    {
        return currentRad / maxRad;
    }

    // run on client
    public void Spawn()
    {
        newRad = startRad;
        if (isLocalPlayer)
            CmdChangeRadius(newRad);
    }
}
