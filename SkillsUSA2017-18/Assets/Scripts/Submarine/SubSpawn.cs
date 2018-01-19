using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SubSpawn : NetworkBehaviour
{
    private Vector2 newPos;
    GameObject mapGen;

    bool spawned;

    // Use this for initialization
    void Start () {
        spawned = false;
        //not fool proof
        mapGen = GameObject.FindGameObjectsWithTag("Map")[0];
    }

    void Update()
    {
        // wait until map is generated
        if (MapGenerator.generated && !spawned) {
            Respawn();
            spawned = true;

            if (isLocalPlayer)
            {
                Camera.main.SendMessage("SetPlayer", gameObject);
            }
        }
    }

    public void Respawn()
    {
        newPos = mapGen.GetComponent<MapGenerator>().GetSpawnPos();
        transform.position = newPos;
    }
}
