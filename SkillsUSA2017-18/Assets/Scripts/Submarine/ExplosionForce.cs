using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExplosionForce : NetworkBehaviour {
    Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    // physics must be run on client
    [ClientRpc]
    public void RpcAddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        
        // blackhole effect fix
        if (wearoff < 0)
        {
            wearoff = 0;
        }
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }
}
