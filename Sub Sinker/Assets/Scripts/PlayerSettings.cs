using UnityEngine;

[System.Serializable]
public class PlayerSettings {
    public string PlayerName;
    public Color PlayerPrimaryColor;
    public Color PlayerSecondaryColor;
    public Color PlayerDecorColor;
    public bool ScrollInvert;
    public bool ControllerEnabled;
    // start at false
    public bool InputIsDisabled;
}
