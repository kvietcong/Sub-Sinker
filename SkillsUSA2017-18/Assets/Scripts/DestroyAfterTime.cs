using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime = 5;
    float et;

	void Start ()
    {
        et = 0;
    }

    private void Update()
    {
        et += Time.deltaTime;
        if (et > destroyTime)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
