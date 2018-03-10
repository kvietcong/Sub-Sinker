using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject MainPauseMenu;
    public GameObject GameSettings;
    public GameObject PlayerSettings;
    public GameObject pickerOne;
    public GameObject pickerTwo;
    public GameObject pickerThree;

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameSettings.activeInHierarchy)
            {
                GameSettings.SetActive(false);
                GameObject player = GameObject.Find("LocalPlayer");
                player.GetComponent<PlayerInfo>().UpdateToServer();
                MainPauseMenu.SetActive(true);
            }
            else if(PlayerSettings.activeInHierarchy)
            {
                if(!pickerOne.activeInHierarchy && !pickerTwo.activeInHierarchy && !pickerThree.activeInHierarchy)
                {
                    PlayerSettings.SetActive(false);
                    GameObject player = GameObject.Find("LocalPlayer");
                    player.GetComponent<PlayerInfo>().UpdateToServer();
                    MainPauseMenu.SetActive(true);
                }
                else
                {
                    pickerOne.SetActive(false);
                    pickerTwo.SetActive(false);
                    pickerThree.SetActive(false);
                }
            }
            else if(MainPauseMenu.activeInHierarchy)
            {
                MainPauseMenu.SetActive(false);
                GameManager.instance.playerSettings.InputIsDisabled = false;
            }
            else
            {
                MainPauseMenu.SetActive(true);
                GameManager.instance.playerSettings.InputIsDisabled = true;
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.Escape))
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
        */
    }
}
