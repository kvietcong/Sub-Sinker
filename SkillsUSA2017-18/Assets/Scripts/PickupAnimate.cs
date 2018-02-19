using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnimate : MonoBehaviour {
    public float yVariance;
    public float moveSpeed;
    public float rotSpeed;

    float startY;

	// Use this for initialization
	void Start () {
        startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(GameManager.instance.t * moveSpeed) * yVariance, transform.position.z);

        transform.rotation = Quaternion.Euler(new Vector3(0, rotSpeed * GameManager.instance.t, 0));
    }
}
