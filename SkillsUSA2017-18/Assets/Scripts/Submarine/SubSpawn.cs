using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SubSpawn : NetworkBehaviour
{
    public GameObject mapGen;

    // Use this for initialization
    void Start () {
        int[,] tileSet = mapGen.getComponent("MapGenerator").map;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
