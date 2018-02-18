using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyPS : MonoBehaviour {
    ParticleSystem ps;
    ParticleSystem.EmissionModule em;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        em = ps.emission;
    }

    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
