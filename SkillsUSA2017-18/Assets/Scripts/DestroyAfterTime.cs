using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime = 5;
	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }
}
