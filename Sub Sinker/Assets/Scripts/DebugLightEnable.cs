using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLightEnable : MonoBehaviour {
    Light debugLight;

    void Start()
    {
        debugLight = GetComponent<Light>();
        debugLight.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (Debug.isDebugBuild)
        {
            // debug light toggle
            if (Input.GetKeyDown(KeyCode.F2))
            {
                debugLight.enabled = !debugLight.enabled;
            }
        }
    }
}
