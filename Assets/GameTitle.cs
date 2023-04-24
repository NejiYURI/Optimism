using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTitle : MonoBehaviour
{
    public void StartGame()
    {
        if (SceneManagerScript.instance) SceneManagerScript.instance.LoadScene("CodingGame");
    }
    public void QuitBtn()
    {
        Application.Quit();
    }
}
