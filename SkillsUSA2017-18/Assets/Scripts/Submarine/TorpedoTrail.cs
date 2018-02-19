using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoTrail : MonoBehaviour {

    public GameObject source;
    bool active;
    float timer;

	// Use this for initialization
	void Start () {
        active = true;
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (source)
            {
                transform.position = source.transform.position + transform.forward * 0.9f;
            }
            else
            {
                var em = GetComponent<ParticleSystem>().emission;
                em.enabled = false;
                active = false;
            }
        }
        else
        {
            if (timer > GetComponent<ParticleSystem>().startLifetime)
            {
                Destroy(gameObject);
            }
            timer += Time.deltaTime;
        }
    }
}
