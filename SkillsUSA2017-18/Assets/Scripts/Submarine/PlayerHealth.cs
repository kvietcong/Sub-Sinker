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
    float respawnProgress;

    public Text timer;

    private void Start()
    {
        barWidth = healthBar.sizeDelta.x;
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

            // todo: play some explosion or something and hide the model
            // note: alive disables PlayerController and Shoot
            alive = false;
            respawnProgress = 0;
            RpcToggleTimer(true);
        }
    }

    private void Update()
    {
        if (!alive)
        {
            if (respawnProgress > respawnTime)
            {
                Respawn();
                if (isServer)
                    RpcReset();
            }
            respawnProgress += Time.deltaTime;
            //if (isServer)
            //{
                RpcEditTimer("Respawn in " + Mathf.Round((respawnTime - respawnProgress) * 10) / 10f);// round to 1 decimal
            //}

        }

        // debug death
        if (Input.GetButton("Jump"))
        {
            if (isLocalPlayer)
                CmdTakeDamage(100);
        }
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        alive = true;
        if (isServer)
        {
            RpcToggleTimer(false);
        }
    }

    void OnChangeHealth(float health)
    {
        healthBar.sizeDelta = new Vector2((health / maxHealth) * barWidth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcReset()
    {
        GetComponent<SubSpawn>().Respawn();
        GetComponent<PlayerInventory>().Respawn();
    }

    [ClientRpc]
    void RpcToggleTimer(bool a)
    {
        timer.enabled = a;
    }

    //[ClientRpc]
    void RpcEditTimer(string str)
    {
        timer.text = str;
    }
}