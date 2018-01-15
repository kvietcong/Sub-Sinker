using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : MonoBehaviour {

    public void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
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
