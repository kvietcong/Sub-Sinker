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

    [SyncVar]
    float respawnProgress;

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

        #region Death
        if (currentHealth <= 0 && alive)
        { 
            currentHealth = 0;

            // todo: play some explosion or something
            // note: alive disables aspects of PlayerMovement, Shoot, EngineLight, and ModelDisable
            alive = false;
            respawnProgress = 0;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            // todo: get colors of subs
            string text = "<color=#0000ff>" + enemyName + "</color> destroyed <color=#0000ff>" + GameManager.instance.playerSettings.PlayerName + "</color>";

            // set killfeed for all players - fix this
            foreach (GameObject player in players)
            {
                if (player.GetComponent<Killfeed>().killfeedText == "")
                {
                    player.GetComponent<Killfeed>().killfeedText = text;
                }
                else
                {
                    player.GetComponent<Killfeed>().killfeedText = text + "\n" + player.GetComponent<Killfeed>().killfeedText;
                }
            }
        }
        #endregion
    }

    private void Update()
    {
        if (!alive)
        {
            if (respawnProgress > ServerManager.instance.respawnTime)
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
                CmdTakeDamage(100, GameManager.instance.playerSettings.PlayerName);
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
        return System.String.Format("Respawn in: {0:F1}", (ServerManager.instance.respawnTime - respawnProgress));
    }
}