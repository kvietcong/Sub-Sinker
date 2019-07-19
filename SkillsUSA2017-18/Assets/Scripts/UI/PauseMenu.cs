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
        string str = playerName.text;
        str = str.Replace(" ", string.Empty);

        // if it is not blank after removing whitespace
        if (str != "")
        {
            GameManager.instance.playerSettings.PlayerName = playerName.text;
        }
	}
}
