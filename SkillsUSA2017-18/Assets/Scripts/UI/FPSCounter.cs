using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "FPS: " + Mathf.RoundToInt(1 / Time.deltaTime);
        float t = (1f / Time.deltaTime - 15f) / 45f;
        text.color = Color.Lerp(Color.red, Color.green, t);
    }
}
