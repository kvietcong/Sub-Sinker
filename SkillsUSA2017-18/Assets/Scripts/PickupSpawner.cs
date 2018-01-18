using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PickupSpawner : NetworkBehaviour {
    public GameObject ammoPrefab;
    bool spawned;

    public GameObject mapGenPrefab;
    GameObject mapGen;

    int pickups;
    public int ammoPerPlayer = 10;

    float ammoSpawnTimer;

    public override void OnStartServer()
    {
        spawned = false;

        // create map generator
        mapGen = Instantiate(mapGenPrefab, new Vector3(), Quaternion.identity);
        NetworkServer.Spawn(mapGen);
        ammoSpawnTimer = Random.Range(2f, 10f);
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
            SpawnAmmo(ammoPerPlayer * 2);
            spawned = true;
        }
        pickups = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        if (pickups < NetworkServer.connections.Count * ammoPerPlayer)
        {
            if (ammoSpawnTimer <= 0)
            {
                SpawnAmmo(1);
                ammoSpawnTimer = Random.Range(2f, 10f);
            }

            // count down when pickups are lacking
            ammoSpawnTimer -= Time.deltaTime;
        }
    }

    void SpawnAmmo(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var spawnPosition = mapGen.GetComponent<MapGenerator>().GetSpawnPos();
            var pickup = (GameObject)Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);
            NetworkServer.Spawn(pickup);
        }
    }
}
