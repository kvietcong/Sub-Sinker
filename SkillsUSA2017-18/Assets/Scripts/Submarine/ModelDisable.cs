using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelDisable : MonoBehaviour {
    public GameObject model;
    public PlayerHealth health;
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
            }
        }
        else
        {
            if (model.activeSelf)
            {
                model.SetActive(false);
            }
        }
    }
}
