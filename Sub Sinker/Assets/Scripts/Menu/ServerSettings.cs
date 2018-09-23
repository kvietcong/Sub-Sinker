using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerSettings : MonoBehaviour
{
    public static ServerManager instance;
    public int mapHeight = 80;
    public int mapWidth = 80;
    public int respawnTime = 3;

    public Slider height;
    public Slider width;
    public Slider respawn;
    public Text widthIndicator;
    public Text heightIndicator;
    public Text respawnIndicator;

    // Use this for initialization
    void Start ()
    {
        height.value = mapHeight;
        width.value = mapWidth;
        respawn.value = respawnTime;
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
        
    }
}
