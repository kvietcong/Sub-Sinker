using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutoDestroyPS : NetworkBehaviour {
    ParticleSystem ps;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    void Update()
    {

        if (ps)
        {
            if (!ps.IsAlive())
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
