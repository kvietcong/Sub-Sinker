using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnTimer : MonoBehaviour {
    PlayerHealth health;
    public Text respawn;
	// Use this for initialization
	void Start () {
        respawn.text = "";
        health = GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		if (health.alive)
        {
            respawn.text = "";
        }
        else
        {
            respawn.text = health.GetTimerText();
        }
	}
}
