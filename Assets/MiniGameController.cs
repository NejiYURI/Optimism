using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadTimer());
    }
    IEnumerator LoadTimer()
    {
        yield return new  WaitForSeconds(3f);
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("MiniGame");
    }
}
