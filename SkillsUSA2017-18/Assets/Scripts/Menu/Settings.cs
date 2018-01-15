using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider musicLevel;
    public GameObject mainMenu;
    public GameObject settingsMenu;

    private void Start()
    {
        if(Screen.width/Screen.height<1.4)
        {
            float scalingFactor = Screen.width / Screen.height;
            mainMenu.transform.localScale = mainMenu.transform.localScale * .75f * scalingFactor;
            settingsMenu.transform.localScale = new Vector3(.8f, .8f, .8f);
        }
    }

    void Update()
    {
        AudioListener.volume = musicLevel.value;
    }

    public void onClick()
    {
        GetComponent<Animator>().SetBool("TrigOn", true);
        GetComponent<Animator>().SetBool("TrigOff", false);
    }

    public void onBack()
    {
        GetComponent<Animator>().SetBool("TrigOff", true);
        GetComponent<Animator>().SetBool("TrigOn", false);
    }

    public void setQuality(int i)
    {
        QualitySettings.SetQualityLevel(i, true);
    }
}
