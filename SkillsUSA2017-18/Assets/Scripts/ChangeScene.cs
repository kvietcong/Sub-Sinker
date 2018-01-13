using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void changeSceneTo(int scene)
    {
        Application.LoadLevel(scene);
    }
}
