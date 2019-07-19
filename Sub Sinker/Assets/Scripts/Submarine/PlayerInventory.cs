using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
    // not the best way of doing this
    public int[] maxAmmo;
    public int[] startAmmo;
    public string[] weapons;
    public int[] currentAmmo;

    public Text display;

	// Use this for initialization
	void Start () {
        Respawn();
	}

    // return true if the operation is valid
    public bool ChangeAmmo(int amount, string weapon)
    {
        int index = System.Array.IndexOf(weapons, weapon);

        // if ammo goes below 0
        if(amount < 0 && currentAmmo[index] + amount < 0)
        {
            return false;
        }

        // if ammo is at/above limit and tries to add more
        else if (currentAmmo[index] >= maxAmmo[index] && amount > 0)
        {
            return false;
        }

        // if ammo is not at limit but would go above
        else if (currentAmmo[index] + amount > maxAmmo[index])
        {
            currentAmmo[index] = maxAmmo[index];
        }

        // standard
        else
        {
            currentAmmo[index] += amount;
        }

        display.text = "Ammo: " + currentAmmo[index] + "/" + maxAmmo[index];
        return true;
    }

    public void Respawn()
    {
        currentAmmo = new int[startAmmo.Length];
        // no check for differing lengths
        for (int i = 0; i < currentAmmo.Length; i++)
        {
            currentAmmo[i] = startAmmo[i] > maxAmmo[i] ? maxAmmo[i] : startAmmo[i];
        }

        // update ui
        ChangeAmmo(0, "torpedo");
    }
}
