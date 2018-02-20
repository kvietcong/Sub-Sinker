using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutoDestroyPS : NetworkBehaviour {
    ParticleSystem ps;

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
