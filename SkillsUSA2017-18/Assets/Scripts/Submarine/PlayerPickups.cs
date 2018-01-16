using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPickups : NetworkBehaviour {
    PlayerInventory ammo;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocalPlayer)
            return;
        if (collision.gameObject.CompareTag("Pick Up"))
        {
            // todo: read ammo crates names to see how much ammo to add
            // ammo is a local value
            if (ammo.ChangeAmmo(2, "torpedo"))
            {
                CmdRemovePickup(collision.gameObject);
            }
        }
    }
    
    [Command]
    void CmdRemovePickup(GameObject pickup)
    {
        Destroy(pickup);
    }

    void Start()
    {
        ammo = GetComponent<PlayerInventory>();
    }
}
