using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Killfeed : NetworkBehaviour
{

    public float timeToCutoff = 5f;
    float timer;

    [SyncVar(hook = "OnChangeText")]
    public string killfeedText = "";

    // Use this for initialization
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (timer >= timeToCutoff)
        {
            timer = 0;

            string[] segs = killfeedText.Split('\n');
            killfeedText = "";
            for (int i = 0; i < segs.Length - 1; i++)
            {
                killfeedText += segs[i];

                // add if not the last one
                if (i != segs.Length - 2)
                {
                    killfeedText += "\n";
                }
            }
        }
        if (killfeedText == "")
        {
            // reset timer when nothing in killfeed
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    void OnChangeText(string text)
    {
        killfeedText = text;
        BroadcastMessage("SetFeed", killfeedText);
    }
}
