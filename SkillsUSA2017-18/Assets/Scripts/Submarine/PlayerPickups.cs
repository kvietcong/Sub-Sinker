using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPickups : NetworkBehaviour {
    PlayerInventory ammo;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
        {
            return;
        }


        if (collision.gameObject.CompareTag("Pick Up"))
        {
            // todo: read ammo crates names to see how much ammo to add
            if (ammo.ChangeAmmo(2, "torpedo"))
            {
                // delete pickup
                Destroy(collision.gameObject);
            }
        }
    }

    void Start()
    {
        ammo = GetComponent<PlayerInventory>();
    }
}
