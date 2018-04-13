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
    public GameObject Background;
    public Toggle ScrollInvert;
    public Toggle Controller;
    public Toggle FPS;
    public Text FPSCounter;

    private void Start()
    {
        FPSCounter = GameObject.Find("FPS").GetComponent<Text>();
        FPS.isOn = GameObject.Find("FPS").activeInHierarchy;
    }

    // Update is called once per frame
    void Update ()
    {
        //
        if (ScrollInvert.isOn)
        {
            GameManager.instance.playerSettings.ScrollInvert = true;
        }
        else
        {
            GameManager.instance.playerSettings.ScrollInvert = false;
        }

        if (Controller.isOn)
        {
            GameManager.instance.playerSettings.ControllerEnabled = true;
        }
        else
        {
            GameManager.instance.playerSettings.ControllerEnabled = false;
        }

        if(FPS.isOn)
        {
            FPSCounter.enabled = true;
        }
        else
        {
            FPSCounter.enabled = false;
        }

        //Exiting and opening
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
                Background.SetActive(false);
                GameManager.instance.playerSettings.InputIsDisabled = false;
            }
            else
            {
                MainPauseMenu.SetActive(true);
                Background.SetActive(true);
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
