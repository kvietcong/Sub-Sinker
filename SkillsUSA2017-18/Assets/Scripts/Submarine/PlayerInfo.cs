using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// this is info specific to each player; it is derived from GameManager
public class PlayerInfo : NetworkBehaviour
{
    [SyncVar]
    [HideInInspector]
    public string playerName;
    [SyncVar(hook ="OnChangePC")]
    [HideInInspector]
    public Color primaryColor;
    [SyncVar(hook = "OnChangeSC")]
    [HideInInspector]
    public Color secondaryColor;

    public GameObject body;

    void Start () {
        SetColors();
        if(isLocalPlayer)
        {
            CmdSetVars(GameManager.instance.playerSettings.PlayerName, GameManager.instance.playerSettings.PlayerPrimaryColor,
                GameManager.instance.playerSettings.PlayerSecondaryColor);
        }
    }

    [Command]
    void CmdSetVars (string name, Color pColor, Color sColor)
    {
        playerName = name;
        primaryColor = pColor;
        secondaryColor = sColor;
    }

    void OnChangePC(Color pc)
    {
        primaryColor = pc;
        SetColors();
    }

    void OnChangeSC(Color sc)
    {
        secondaryColor = sc;
        SetColors();
    }

    public void SetColors()
    {
        body.GetComponent<MeshRenderer>().materials[0].color = primaryColor;
        body.GetComponent<MeshRenderer>().materials[1].color = secondaryColor;
    }
}
