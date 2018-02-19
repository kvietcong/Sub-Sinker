using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{

    Vector3 driftDir;
    public float forceMultiplier;
    public float gravMultiplier;
    Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        driftDir = new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f, 0);
        driftDir.Normalize();
        body = gameObject.GetComponent<Rigidbody2D>();
        print(driftDir);
    }

    // Update is called once per frame
    void Update()
    {
        body.AddForce(driftDir * forceMultiplier * Time.deltaTime);

        // gravity
        body.AddForce(new Vector3(0, -1 * gravMultiplier * Time.deltaTime, 0));

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Map")
        {
            driftDir = new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f, 0);
        }
    }
}
