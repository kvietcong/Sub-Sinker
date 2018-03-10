using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public Slider musicLevel;

	// Update is called once per frame
	void Update ()
    {
        AudioListener.volume = musicLevel.value;
    }
}
