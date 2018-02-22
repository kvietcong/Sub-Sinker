using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject PlayerSettings;

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerSettings.active)
            {
                PlayerSettings.SetActive(false);
                GameObject player = GameObject.Find("LocalPlayer");
                player.GetComponent<PlayerInfo>().UpdateToServer();
            }
            else
            {
                PlayerSettings.SetActive(true);
            }
        }
    }
}
