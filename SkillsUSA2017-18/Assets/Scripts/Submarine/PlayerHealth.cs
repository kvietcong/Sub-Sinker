using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// health is managed almost entirely by the server
public class PlayerHealth : NetworkBehaviour
{
    public const float maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public float currentHealth;
    public RectTransform healthBar;
    float barWidth;
    [SyncVar]
    public bool alive;

    public float respawnTime;
    [SyncVar]
    float respawnProgress;

    private void Start()
    {
        barWidth = healthBar.sizeDelta.x;
        if (isServer)
            Respawn();
    }

    [Command]
    public void CmdTakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0 && alive)
        { 
            // dead
            currentHealth = 0;

            // todo: play some explosion or something and hide the model -- in other scripts
            // note: alive disables aspects of PlayerController, Shoot, EngineLight
            alive = false;
            respawnProgress = 0;
        }
    }

    private void Update()
    {
        if (!alive)
        {
            if (respawnProgress > respawnTime)
            {
                if (isServer)
                {
                    Respawn();
                    RpcReset();
                }
                    
            }
            respawnProgress += Time.deltaTime;
        }

        // debug death
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (isLocalPlayer)
            {
                CmdTakeDamage(100);
            }
        }
        print("Health: " + currentHealth);
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        alive = true;
    }

    void OnChangeHealth(float health)
    {
        currentHealth = health;
        healthBar.sizeDelta = new Vector2((health / maxHealth) * barWidth, healthBar.sizeDelta.y);
        print("Changed health: " + currentHealth + ", " + health);
    }

    [ClientRpc]
    void RpcReset()
    {
        GetComponent<SubSpawn>().Respawn();
        GetComponent<PlayerInventory>().Respawn();
        GetComponent<EngineLight>().Spawn();
    }

    public string GetTimerText()
    {
        return System.String.Format("Respawn in: {0:F1}", (respawnTime - respawnProgress));
    }
}