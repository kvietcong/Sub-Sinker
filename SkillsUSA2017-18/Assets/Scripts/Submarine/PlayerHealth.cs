using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{
    public const float maxHealth = 100;
    public float currentHealth = maxHealth;

   public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }
    }
}