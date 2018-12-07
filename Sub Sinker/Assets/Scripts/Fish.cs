using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
    Rigidbody2D rb;
    public float forceScale;
    float baseSpeed;
    int dir; // -1 = left
	// Use this for initialization
	void Start () {
        Color jellyColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 0.75f, 0.75f);
        GetComponent<MeshRenderer>().materials[0].color = jellyColor;
        gameObject.transform.localScale = new Vector3(Random.Range(1f, 3f), Random.Range(1f, 3f), Random.Range(1f, 3f));

        rb = gameObject.GetComponent<Rigidbody2D>();

        dir = Random.Range(0, 1) * 2 - 1;
        baseSpeed = Random.Range(0, 15f);
    }
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(new Vector2(Random.Range(baseSpeed - 2f, baseSpeed + 2f) * Time.deltaTime * dir, 0) * forceScale);

        if (dir == -1)
        {
            transform.eulerAngles = new Vector3();
        }
        else if (dir == 1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        dir *= -1;
    }
}
