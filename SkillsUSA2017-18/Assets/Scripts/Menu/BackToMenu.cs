using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BackToMenu : NetworkBehaviour {
    public Canvas canvas;

    public void OnStartServer()
    {
        print(canvas.enabled);
        canvas.enabled = false;
    }
    public void OnClientDisconnect()
    {
        canvas.enabled = true;
    }
}
