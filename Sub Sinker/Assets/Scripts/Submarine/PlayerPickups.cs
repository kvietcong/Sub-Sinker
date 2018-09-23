using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPickups : NetworkBehaviour {
    PlayerInventory ammo;
    PlayerHealth health;

    public int ammoAmount;
    public float healthAmount;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocalPlayer)
            return;
        if (collision.gameObject.CompareTag("Pick Up"))
        {
            // todo: read ammo crates names to see how much ammo to add
            // ammo is a local value
            if (collision.gameObject.name == "AmmoPickup(Clone)")
            {
                if (ammo.ChangeAmmo(ammoAmount, "torpedo"))
                {
                    CmdRemovePickup(collision.gameObject);
                }
            }
            else if (collision.gameObject.name == "HealthPickup(Clone)")
            {
                if (health.currentHealth < PlayerHealth.maxHealth)
                {
                    CmdRemovePickup(collision.gameObject);
                    CmdChangeHealth(healthAmount);
                }
            }
        }
    }
    
    [Command]
    void CmdRemovePickup(GameObject pickup)
    {
        Destroy(pickup);
    }

    [Command]
    void CmdChangeHealth(float amt)
    {
        health.currentHealth += amt;
        if (health.currentHealth > PlayerHealth.maxHealth)
        {
            health.currentHealth = PlayerHealth.maxHealth;
        }
    }
    void Start()
    {
        ammo = GetComponent<PlayerInventory>();
        health = GetComponent<PlayerHealth>();
    }
}
