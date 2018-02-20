using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Jellyfish : NetworkBehaviour
{

    Vector3 driftDir;
    public float driftForceMultiplier;
    public float driftChangeRate;
    public float gravMultiplier;
    public float pushForce;
    public float pushForceVariance;
    public float pushDelay;
    public float pushDelayVariance;
    float timer;
    float delay;
    Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        // random color
        Color jellyColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 0.75f, 0.75f);
        GetComponent<MeshRenderer>().materials[0].color = jellyColor;
        GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", jellyColor);
        print(GetComponent<MeshRenderer>().materials[0].color);
        if (!isServer)
        {
            return;
        }
        driftDir = newDir();
        body = gameObject.GetComponent<Rigidbody2D>();
        timer = 0;
        delay = Random.Range(0, pushDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }
        body.AddForce(driftDir * driftForceMultiplier * Time.deltaTime);

        // gravity
        body.AddForce(new Vector3(0, -1 * gravMultiplier * Time.deltaTime, 0));

        if (timer >= delay)
        {
            timer = 0;
            body.AddForce(new Vector3(0, pushForce * Random.Range(1f - pushForceVariance, 1f + pushForceVariance), 0));
            delay = pushDelay * Random.Range(1f - pushDelayVariance, 1f + pushDelayVariance);
        }

        // cap
        float newX = driftDir.x + driftChangeRate * Time.deltaTime * ((UnityEngine.Random.value - 0.5f) * 2);
        if (newX >= 1)
        {
            newX = 1;
        }
        else if (newX <= -1)
        {
            newX = -1;
        }
            
        driftDir = new Vector3(newX, 0, 0);
        timer += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!isServer)
        {
            return;
        }
        if (coll.gameObject.tag == "Map")
        {
            driftDir = newDir();
        }
    }

    Vector3 newDir()
    {
        Vector3 dir = new Vector3((UnityEngine.Random.value - 0.5f) * 2, 0, 0);
        return dir;
    }
}
