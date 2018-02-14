using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// health is managed almost entirely by the server
// note: PlayerPickups directly changes health
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

    public string playerName = "Player";
    public Text killfeed;

    private void Start()
    {
        barWidth = healthBar.sizeDelta.x;
        if (isServer)
        {
            Respawn();
        }
            
    }

    [Command]
    public void CmdTakeDamage(float amount, string enemyName)
    {
        currentHealth -= amount;
        if (currentHealth <= 0 && alive)
        { 
            // dead
            currentHealth = 0;

            // todo: play some explosion or something, disable collider
            // note: alive disables aspects of PlayerController, Shoot, EngineLight
            alive = false;
            respawnProgress = 0;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            // set killfeed for all players
            foreach (GameObject player in players)
            {
                // if empty, simply set text
                if (player.GetComponent<Killfeed>().killfeedText == "")
                {
                    player.GetComponent<Killfeed>().killfeedText =
                    "<color=#0000ff>" + enemyName + "</color> destroyed <color=#0000ff>" + playerName + "</color>";
                }
                else
                {
                    player.GetComponent<Killfeed>().killfeedText =
                        "<color=#0000ff>" + enemyName + "</color> destroyed <color=#0000ff>" + playerName + "</color>\n" +
                        player.GetComponent<Killfeed>().killfeedText;
                }
            }
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
                CmdTakeDamage(100, playerName);
            }
        }
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