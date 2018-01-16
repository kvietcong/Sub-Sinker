using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public const float maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public float currentHealth;
    public RectTransform healthBar;
    float barWidth;
    public bool alive;

    public float respawnTime;
    float respawnProgress;

    public Text timer;

    private void Start()
    {
        barWidth = healthBar.sizeDelta.x;
        Respawn();
        timer.enabled = false;
    }

    public void TakeDamage(float amount)
    {
        if (!isServer)
        {
            return;
        }
        currentHealth -= amount;
        if (currentHealth <= 0)
        { 
            // dead
            currentHealth = 0;

            // todo: play some explosion or something and hide the model
            // note: alive disables PlayerController and Shoot
            alive = false;
            respawnProgress = 0;
            timer.enabled = true;
        }
    }

    private void Update()
    {
        if (!alive)
        {
            if (respawnProgress > respawnTime)
            {
                GetComponent<SubSpawn>().Respawn();
                GetComponent<PlayerInventory>().Respawn();
                Respawn();
            }
            respawnProgress += Time.deltaTime;
            timer.text = "Respawn in " + Mathf.Round((respawnTime - respawnProgress) * 10) / 10f;// round to 1 decimal
        }
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        alive = true;
        timer.enabled = false;
    }

    void OnChangeHealth(float health)
    {
        healthBar.sizeDelta = new Vector2((health / maxHealth) * barWidth, healthBar.sizeDelta.y);
    }
}