using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider musicLevel;
    public Slider sensitivityLevel;

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
