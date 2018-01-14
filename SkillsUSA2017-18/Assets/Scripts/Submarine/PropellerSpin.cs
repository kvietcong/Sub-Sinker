using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerSpin : MonoBehaviour {
    float vel;
    float speed;

    // Use this for initialization
    private void Start()
    {
        speed = 5;
    }
    void Update () {
        transform.Rotate(vel * speed, 3, 0);
	}
	
	// Update is called once per frame
	void AdjustVel (float newVel) {
        vel = newVel;
	}
}
