using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SubSpawn : NetworkBehaviour
{
    public GameObject mapGenPrefab;
    private Vector2 newPos;
    GameObject mapGen;

    bool spawned;

    // Use this for initialization
    void Start () {
        spawned = false;

        // create map generator on server only
        if (isServer)
        {
            mapGen = Instantiate(mapGenPrefab, new Vector3(), Quaternion.identity);
            NetworkServer.Spawn(mapGen);
        }
    }

    void Update()
    {
        // wait until map is generated
        if (MapGenerator.generated && !spawned) { 
            newPos = mapGen.GetComponent<MapGenerator>().GetSpawnPos();
            transform.position = newPos;
            spawned = true;

            print(newPos);
        }
    }
}
