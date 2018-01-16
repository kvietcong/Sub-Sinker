using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PickupSpawner : NetworkBehaviour {
    public GameObject ammoPrefab;
    public int numberOfPickups;

    bool spawned;

    public GameObject mapGenPrefab;
    GameObject mapGen;

    public override void OnStartServer()
    {
        spawned = false;

        // create map generator
        mapGen = Instantiate(mapGenPrefab, new Vector3(), Quaternion.identity);
        NetworkServer.Spawn(mapGen);
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }
        // wait until map is generated
        if (MapGenerator.generated && !spawned)
        {
            for (int i = 0; i < numberOfPickups; i++)
            {
                var spawnPosition = mapGen.GetComponent<MapGenerator>().GetSpawnPos();
                var pickup = (GameObject)Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);
                NetworkServer.Spawn(pickup);
            }
            spawned = true;
        }
    }
}
