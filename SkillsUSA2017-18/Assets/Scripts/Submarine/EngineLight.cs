using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EngineLight : NetworkBehaviour {
    public Light engineLight;

    public const float startRad = 10;

    [SyncVar(hook = "OnLightRadiusChange")]
    public float currentRad = startRad;
    private float newRad;
    private float scrollSpeed;

    public float maxRad = 25;
    public float minRad = 2;
    private float startIntensity;

    public RectTransform circle;
    public GameObject model;
    MeshRenderer[] rends;
    Text debugText;

    GameObject localPlayer;

    // Use this for initialization
    void Start () {
        newRad = startRad;
        engineLight.range = newRad;
        scrollSpeed = 10;
        startIntensity = engineLight.intensity;
        debugText = GameObject.Find("Debug").GetComponentInChildren<Text>();
        rends = model.GetComponentsInChildren<MeshRenderer>();
        localPlayer = GameObject.Find("LocalPlayer");
    }

    // Update is called once per frame
    void Update() {
        // i eyeballed this value....
        if(Vector3.Distance(transform.position, localPlayer.transform.position) > currentRad * currentRad * 0.08f)
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
        currentRad = radius;
        engineLight.range = radius;
        engineLight.spotAngle = radius * 7;
        circle.sizeDelta = new Vector2(radius * radius * 2.8f, radius * radius * 2.8f);
    }
}
