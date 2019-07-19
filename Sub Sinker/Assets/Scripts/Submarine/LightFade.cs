    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LightFade : NetworkBehaviour {
    Light lt;
    public float timeToFade = 1f;
    float initIntens;
    float elapsedTime;

	// Use this for initialization
	void Start () {
        lt = GetComponent<Light>();
        elapsedTime = 0;
        initIntens = lt.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        if (elapsedTime >= timeToFade)
        {
            Destroy(this.gameObject);
        }
        lt.intensity = Mathf.Lerp(initIntens, 0, elapsedTime / timeToFade);
        elapsedTime += Time.deltaTime;
	}
}
