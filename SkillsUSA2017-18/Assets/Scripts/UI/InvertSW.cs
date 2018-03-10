using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertSW : MonoBehaviour
{
    public Toggle scrollToggle;

    public void Update()
    {
        if(scrollToggle.isOn)
        {
            GameManager.instance.playerSettings.ScrollInvert = true;
        }
        else
        {
            GameManager.instance.playerSettings.ScrollInvert = false;
        }
    }
}
