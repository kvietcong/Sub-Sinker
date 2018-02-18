using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PickupSpawner : NetworkBehaviour {
    public static PickupSpawner instance;

    public GameObject serverManagerPrefab;
    public GameObject ammoPrefab;
    public GameObject healthPrefab;
    bool spawned;

    GameObject[] pickups;
    public int ammoPerPlayer = 10;
    public int healthPerPlayer = 5;
    int ammos;
    int healths;

    float ammoSpawnTimer;
    float healthSpawnTimer;

    public override void OnStartServer()
    {
        if (instance != null)
        {
            Debug.LogError("More than one PickupSpawner in scene.");
        }
        else
        {
            instance = this;
        }

        spawned = false;
        GameObject serverManager = Instantiate(serverManagerPrefab, new Vector3(), Quaternion.identity);
        NetworkServer.Spawn(serverManager);

        ammoSpawnTimer = Random.Range(2f, 10f);
        healthSpawnTimer = Random.Range(2f, 10f);
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
            SpawnPickup(ammoPerPlayer * 2, ammoPrefab);
            SpawnPickup(healthPerPlayer * 2, healthPrefab);
            spawned = true;
        }

        #region Count pickups
        pickups = GameObject.FindGameObjectsWithTag("Pick Up");
        ammos = 0;
        healths = 0;
        foreach (GameObject pickup in pickups)
        {
            if (pickup.name == "AmmoPickup(Clone)")
            {
                ammos++;
            }
            else if (pickup.name == "HealthPickup(Clone)")
            {
                healths++;
            }
        }
        #endregion

        #region Spawn pickups when lacking
                if (ammos < NetworkServer.connections.Count * ammoPerPlayer)
                {
                    if (ammoSpawnTimer <= 0)
                    {
                        SpawnPickup(1, ammoPrefab);
                        ammoSpawnTimer = Random.Range(2f, 10f);
                    }

                    // count down when pickups are lacking
                    ammoSpawnTimer -= Time.deltaTime;
                }
                if (healths < NetworkServer.connections.Count * healthPerPlayer)
                {
                    if (healthSpawnTimer <= 0)
                    {
                        SpawnPickup(1, healthPrefab);
                        healthSpawnTimer = Random.Range(2f, 10f);
                    }

                    // count down when pickups are lacking
                    healthSpawnTimer -= Time.deltaTime;
                }
        #endregion
    }

    void SpawnPickup(int num, GameObject prefab)
    {
        for (int i = 0; i < num; i++)
        {
            var spawnPosition = MapGenerator.instance.GetComponent<MapGenerator>().GetSpawnPos();
            var pickup = (GameObject)Instantiate(prefab, spawnPosition, Quaternion.identity);
            NetworkServer.Spawn(pickup);
        }
    }

    public void DestroyPickups()
    {
        pickups = GameObject.FindGameObjectsWithTag("Pick Up");
        foreach (GameObject pickup in pickups)
        {
            NetworkServer.Destroy(pickup);
        }
    }
}
