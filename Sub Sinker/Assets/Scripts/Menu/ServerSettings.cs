using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// IMPORTANT NOTE
// WIDTH indicator infact comes from the height value in the UI, and vice versa
// this is because the map generator in fact uses height as width, and width and height
// thus it must be re-reversed in the UI
public class ServerSettings : MonoBehaviour
{
    public Slider height;
    public Slider width;
    public Slider respawn;
    public Text widthIndicator;
    public Text heightIndicator;
    public Text respawnIndicator;
    public InputField seedIndicator;
    public Toggle randIndicator;

    // Use this for initialization
    void Start ()
    {
        height.value = ServerManager.instance.mapHeight;
        width.value = ServerManager.instance.mapWidth;
        respawn.value = ServerManager.instance.respawnTime;
        seedIndicator.text = ServerManager.instance.mapSeed;
        randIndicator.isOn = ServerManager.instance.randomMapSeed;
    }
	
	// Update is called once per frame
	void Update ()
    {
        widthIndicator.text = width.value.ToString();
        heightIndicator.text = height.value.ToString();
        respawnIndicator.text = respawn.value.ToString();
    }

    public void Apply()
    {
        if (ServerManager.instance.mapWidth != (int)width.value)
        {
            ServerManager.instance.mapWidth = (int)width.value;
        }
        if (ServerManager.instance.mapHeight != (int)height.value)
        {
            ServerManager.instance.mapHeight = (int)height.value;
        }
        if (ServerManager.instance.respawnTime != (int)respawn.value)
        {
            ServerManager.instance.respawnTime = (int)respawn.value;
        }
        if (ServerManager.instance.mapSeed != (string)seedIndicator.text)
        {
            ServerManager.instance.mapSeed = (string)seedIndicator.text;
        }
        if (ServerManager.instance.randomMapSeed != (bool)randIndicator.isOn)
        {
            ServerManager.instance.randomMapSeed = (bool)randIndicator.isOn;
        }
    }

    public void Regenerate()
    {
        //Apply(); // decided not to include this since it would generate the map twice.
        // not sure if its confusing that it would use old values but whatever
        ServerManager.instance.RegenerateMap();
    }
}
