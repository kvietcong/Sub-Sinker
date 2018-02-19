using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    [SyncVar(hook = "OnChangeDC")]
    [HideInInspector]
    public Color decorColor;

    public GameObject body;
    public GameObject periscope;
    public GameObject propeller;
    public GameObject nametag;

    void Start () {
        SetColors();
        if(isLocalPlayer)
        {
            UpdateToServer();
        }
    }

    public void UpdateToServer()
    {
        CmdSetVars(GameManager.instance.playerSettings.PlayerName, GameManager.instance.playerSettings.PlayerPrimaryColor,
                GameManager.instance.playerSettings.PlayerSecondaryColor, GameManager.instance.playerSettings.PlayerDecorColor);
    }

    [Command]
    void CmdSetVars (string name, Color pColor, Color sColor, Color dColor)
    {
        playerName = name;
        primaryColor = pColor;
        secondaryColor = sColor;
        decorColor = dColor;
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

    void OnChangeDC(Color dc)
    {
        decorColor = dc;
        SetColors();
    }

    public void SetColors()
    {
        // main
        body.GetComponent<MeshRenderer>().materials[0].color = primaryColor;
        // stripe + fins
        body.GetComponent<MeshRenderer>().materials[1].color = secondaryColor;
        // window frames
        body.GetComponent<MeshRenderer>().materials[4].color = decorColor;
        // propeller attachment
        body.GetComponent<MeshRenderer>().materials[2].color = secondaryColor;
        periscope.GetComponent<MeshRenderer>().material.color = decorColor;
        propeller.GetComponent<MeshRenderer>().material.color = decorColor;

        nametag.GetComponent<Text>().text = "<b><color=#" + ColorUtility.ToHtmlStringRGB(primaryColor) + ">" + playerName + "</color></b>";
    }
}
