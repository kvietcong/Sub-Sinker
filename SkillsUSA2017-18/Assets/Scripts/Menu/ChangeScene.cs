using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void changeSceneTo(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            SceneManager.LoadScene(0);
        }
    }
}
