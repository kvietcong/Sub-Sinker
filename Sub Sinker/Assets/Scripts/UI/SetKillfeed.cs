using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetKillfeed : MonoBehaviour {

	void SetFeed(string theText)
    {
        gameObject.GetComponent<Text>().text = theText;
    }
}
