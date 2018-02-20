using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public Text playerName;

	// Update is called once per frame
	void Update ()
    {
        GameManager.instance.playerSettings.PlayerName = playerName.text;
	}
}
