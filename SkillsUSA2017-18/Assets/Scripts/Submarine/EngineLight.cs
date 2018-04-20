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

    public GameObject nametag;
    public GameObject bubbles;

    public override void OnStartClient()
    {
        AdjustEngineLight(currentRad);
    }

    void Start () {
        localPlayer = GameObject.Find("LocalPlayer");
        scrollSpeed = 10;
        startIntensity = engineLight.intensity;
        health = GetComponent<PlayerHealth>();
        Spawn();
    }

    void Update() {
        if (!health.alive)
        {
            // vvvv   maybe use this instead   vvvv (but does it matter)
            //engineLight.intensity = 0;
            if (currentRad != 0 && isLocalPlayer)
            {
                AdjustEngineLight(0);
                CmdChangeRadius(0); // light off
            }
            nametag.SetActive(false);
            return;
        }

        // represents the far left side of the circle
        //Vector3 circleVector = (Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2 + currentRad * currentRad * 2.8f, Camera.main.pixelHeight / 2, Camera.main.transform.position.z)));

        //// equalize z values
        //circleVector = new Vector3(circleVector.x, circleVector.y, localPlayer.transform.position.z);
       
        
        //float dist = Vector3.Distance(circleVector, transform.position);

        //print(currentRad * currentRad * 0.08f);
        //print(dist + " dist");

        // i used the eyeball values, which were vindicated by my own experiment (which is annoying to impliment)
        if (Vector3.Distance(transform.position, localPlayer.transform.position) > currentRad * currentRad * 0.08f)
        {
            // hide light/nametag when not within distance of localplayer
            engineLight.intensity = 0;
            nametag.SetActive(false);
            bubbles.SetActive(false);
        }
        else
        {
            engineLight.intensity = startIntensity;
            nametag.SetActive(true);
            bubbles.SetActive(true);
        }

        // network awareness
        if (!isLocalPlayer)
        {
            return;
        }

        if (GameManager.instance.playerSettings.InputIsDisabled)
        {
            return;
        }

        if (GameManager.instance.playerSettings.ControllerEnabled)
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

        if (GameManager.instance.playerSettings.ScrollInvert)
        {
            // do the math. it inverts it
            newRad = 2 * currentRad - newRad;
        }

        if (newRad < minRad) {
            newRad = minRad;
        }
        else if (newRad > maxRad) {
            newRad = maxRad;
        }

        if (newRad != currentRad) {
            // change the light instantaneously, so you dont have to wait for the server
            AdjustEngineLight(newRad);
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
        if (!isLocalPlayer)
        {
            AdjustEngineLight(radius);
        }
    }

    public float GetLightMultiplier()
    {
        return currentRad / maxRad;
    }

    void AdjustEngineLight(float r)
    {
        engineLight.range = r;
        engineLight.spotAngle = r * 7;
        circle.sizeDelta = new Vector2(r * r * 2.8f, r * r * 2.8f);
    }

    // run on client
    public void Spawn()
    {
        newRad = startRad;
        if (isLocalPlayer)
        {
            CmdChangeRadius(newRad);
            AdjustEngineLight(newRad);
        }
    }
}
