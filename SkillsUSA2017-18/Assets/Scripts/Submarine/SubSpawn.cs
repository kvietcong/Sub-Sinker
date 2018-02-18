using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SubSpawn : NetworkBehaviour
{
    private Vector2 newPos;
    public GameObject body;
    bool spawned;

    // Use this for initialization
    void Start () {
        spawned = false;
        body.GetComponent<MeshRenderer>().materials[0].color = GameManager.instance.playerSettings.PlayerPrimaryColor;
        body.GetComponent<MeshRenderer>().materials[1].color = GameManager.instance.playerSettings.PlayerSecondaryColor;
        //not fool proof :/
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
        newPos = MapGenerator.instance.GetComponent<MapGenerator>().GetSpawnPos();
        transform.position = newPos;
    }
}
