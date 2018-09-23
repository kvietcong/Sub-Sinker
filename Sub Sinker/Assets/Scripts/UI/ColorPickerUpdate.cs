using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUpdate : MonoBehaviour
{

    public Image primary;
    public Image secondary;
    public Image decor;

    // Update is called once per frame
    void Update ()
    {
        primary.color = GameManager.instance.playerSettings.PlayerPrimaryColor;
        secondary.color = GameManager.instance.playerSettings.PlayerSecondaryColor;
        decor.color = GameManager.instance.playerSettings.PlayerDecorColor;
    }
}
