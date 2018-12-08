// this is a script for storing server values -- only the server can set these
// however, it is a shared object because its values need to be read by clients

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : NetworkBehaviour {
    public static ServerManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one ServerManager in scene.");
        }
        else
        {
            instance = this;
        }
    }

    [SyncVar]
    public float respawnTime;
    //public GameMode gameMode;
    // currently not functional
    public int serverSize;
    [SyncVar(hook = "OnChangeWidth")]
    public int mapWidth; // set minimums
    [SyncVar(hook = "OnChangeHeight")]
    public int mapHeight;
    [SyncVar(hook = "OnChangeRandomSeed")]
    public bool randomMapSeed;

    // mapSeed: this stays the same if random is checked
    [SyncVar(hook = "OnChangeSeed")]
    public string mapSeed;

    // tempSeed: changes depending on whether random is checked or not
    [HideInInspector]
    [SyncVar(hook = "OnChangeTemp")]
    public string tempSeed;

    public GameObject mapGenPrefab;

    private void Start()
    {
        if (isServer)
        {
            // temp seed generates new map -> will result in warning
            SetTempSeed();
            // map generates on instantiate
            GameObject mapGen = Instantiate(mapGenPrefab, new Vector3(), Quaternion.identity);
            NetworkServer.Spawn(mapGen);
        }
    }

    // run on server and clients
    public void RegenerateMap()
    {
        if (MapGenerator.instance)
        {
            SetTempSeed();
            MapGenerator.instance.GetComponent<MapGenerator>().GenerateMap(tempSeed);
            if (isServer)
            {
                PickupSpawner.instance.DestroyPickups();
                PickupSpawner.instance.InitSpawnPickups();
            }
            // respawn local player
            // expected beginning error
            GameObject.Find("LocalPlayer").GetComponent<SubSpawn>().Respawn();
        }
        else
        {
            // expected at start due to temp seed hook
            Debug.LogWarning("MapGenerator not instantiated");
        }
    }

    void SetTempSeed()
    {
        if (isServer)
        {
            tempSeed = randomMapSeed ? UnityEngine.Random.value.ToString() : mapSeed;
        }
    }

    #region SyncVar hooks
    void OnChangeWidth(int width)
    {
        mapWidth = width;
        RegenerateMap();
    }

    void OnChangeHeight(int height)
    {
        mapHeight = height;
        RegenerateMap();
    }

    void OnChangeRandomSeed(bool rand)
    {
        randomMapSeed = rand;
        if (isServer)
        {
            if (randomMapSeed)
            {
                // change seed if it is now random
                tempSeed = UnityEngine.Random.value.ToString();
            }
            else
            {
                tempSeed = mapSeed;
            }

            // i had to do this because it doesn't call it on the server... maybe because it can't call a new hook within this hook?
            OnChangeTemp(tempSeed);
        }
    }

    void OnChangeSeed(string seed)
    {
        mapSeed = seed;
        if (!randomMapSeed)
        {
            // apparently, changing temp seed here does not trigger a RegenerateMap
            tempSeed = mapSeed;
            RegenerateMap();
        }
    }

    void OnChangeTemp(string seed)
    {
        tempSeed = seed;
        RegenerateMap();
    }
    #endregion
}
