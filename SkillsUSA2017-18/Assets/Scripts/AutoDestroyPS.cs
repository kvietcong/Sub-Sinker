using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyPS : MonoBehaviour {
    ParticleSystem ps;

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
