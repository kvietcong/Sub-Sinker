using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelDisable : MonoBehaviour {
    public GameObject model;
    PlayerHealth health;

	// Use this for initialization
	void Start () {
        model.SetActive(true);
        health = GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		if (health.alive)
        {
            if (!model.activeSelf)
            {
                model.SetActive(true);
                // enable colliders
                foreach (Collider2D c in GetComponents<Collider2D>())
                {
                    c.isTrigger = false;
                }
            }
        }
        else
        {
            if (model.activeSelf)
            {
                model.SetActive(false);
                // disable colliders
                foreach (Collider2D c in GetComponents<Collider2D>())
                {
                    c.isTrigger = true;
                }
            }
        }
    }
}
