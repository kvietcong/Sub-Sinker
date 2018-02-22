using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject PlayerSettings;
    public GameObject pickerOne;
    public GameObject pickerTwo;
    public GameObject pickerThree;

    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerSettings.activeInHierarchy && !pickerOne.activeInHierarchy && !pickerTwo.activeInHierarchy && !pickerThree.activeInHierarchy)
            {
                PlayerSettings.SetActive(false);
                GameObject player = GameObject.Find("LocalPlayer");
                player.GetComponent<PlayerInfo>().UpdateToServer();
                GameManager.instance.playerSettings.InputIsDisabled = false;
            }
            else
            {
                PlayerSettings.SetActive(true);
                GameManager.instance.playerSettings.InputIsDisabled = true;
            }
        }
    }
}
