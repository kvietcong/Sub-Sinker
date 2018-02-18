// this is a script for storing server values -- only the server can set these
// however, it is a shared object because its values need to be read by clients

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : NetworkBehaviour {
    public static ServerManager instance;

    [SyncVar]
    public float respawnTime;
    //public GameMode gameMode;
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
    // doesn't need hook because it will only be changed when randomseed is checked
    [HideInInspector]
    [SyncVar]
    public string tempSeed;

    public GameObject mapGenPrefab;

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

        // create map generator
        if (isServer)
        {
            if (randomMapSeed)
            {
                tempSeed = UnityEngine.Random.value.ToString();
            }
        }
    }

    private void Start()
    {
        if (isServer)
        {
            GameObject mapGen = Instantiate(mapGenPrefab, new Vector3(), Quaternion.identity);
            NetworkServer.Spawn(mapGen);
        }
    }

    // run on server and clients
    void RegenerateMap()
    {
        if (isServer)
        {
            PickupSpawner.instance.DestroyPickups();
        }
        MapGenerator.instance.GetComponent<MapGenerator>().GenerateMap(tempSeed);
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
            {// change seed if it is now random
                tempSeed = UnityEngine.Random.value.ToString();
            }
            else
            {
                tempSeed = mapSeed;
            }
        }
        RegenerateMap();
    }

    void OnChangeSeed(string seed)
    {
        if (!randomMapSeed)
        {
            mapSeed = seed;
        }
        tempSeed = mapSeed;
        RegenerateMap();
    }
    #endregion
}
