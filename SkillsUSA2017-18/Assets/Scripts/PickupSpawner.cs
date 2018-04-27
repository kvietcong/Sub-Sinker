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
    public GameObject jellyPrefab;
    public GameObject shipPrefab;
    public GameObject seaweedPrefab;
    public GameObject fishPrefab;
    bool spawned;

    GameObject[] pickups;
    public int ammoPerPlayer = 10;
    public int healthPerPlayer = 5;
    public int jellyPerPlayer = 15;
    public int ships = 2;
    public int seaweeds = 30;
    public int fishPerPlayer = 50;
    int ammos;
    int healths;

    float ammoSpawnTimer;
    float healthSpawnTimer;

    // universal value for pickup animations
    public float t;

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

        t = 0;
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
            InitSpawnPickups();
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

        t += Time.deltaTime;
    }

    public void InitSpawnPickups()
    {
        SpawnPickup(ammoPerPlayer, ammoPrefab);
        SpawnPickup(healthPerPlayer, healthPrefab);
        SpawnPickup(jellyPerPlayer, jellyPrefab);
        SpawnPickup(fishPerPlayer, fishPrefab);
        SpawnPickup(seaweeds, seaweedPrefab, true);
        SpawnPickup(ships, shipPrefab, true);
    }

    void SpawnPickup(int num, GameObject prefab, bool ground = false)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 spawnPosition;
            if (ground)
            {
                spawnPosition = MapGenerator.instance.GetComponent<MapGenerator>().GetGroundSpawnPos();
            }
            else
            {
                spawnPosition = MapGenerator.instance.GetComponent<MapGenerator>().GetSpawnPos();
            }

            if (LayerMask.LayerToName(prefab.layer) == "Environment")
            {
                // put it behind map
                spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 2f);
            }
            var pickup = (GameObject)Instantiate(prefab, spawnPosition, Quaternion.identity);

            // bad and lazy :(
            if (pickup.name == "seaweed(Clone)")
            {
                pickup.transform.eulerAngles = new Vector3(270f, 90f, 0);
                pickup.transform.position += new Vector3(0, 1.6f, 0);
            }
            else if (pickup.name == "garbageship(Clone)")
            {
                pickup.transform.eulerAngles = new Vector3(Random.Range(-10f, 10f), -90f, 0);
                pickup.transform.position += new Vector3(0, Random.Range(0.5f, 2f), 0.57f);
            }
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

        pickups = GameObject.FindGameObjectsWithTag("Environment");
        foreach (GameObject pickup in pickups)
        {
            NetworkServer.Destroy(pickup);
        }
    }
}
