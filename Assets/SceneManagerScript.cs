using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    [SerializeField]
    private string CurrentOpenScene;

    private void Awake()
    {
        if (SceneManagerScript.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAdditive(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    public void UnloadScene(string sceneName)
    {
        StartCoroutine(UnloadLevel(sceneName));
    }

    public void UnloadCurrentScene()
    {
        if (!string.IsNullOrEmpty(CurrentOpenScene))
        {
            StartCoroutine(UnloadLevel(CurrentOpenScene));
            CurrentOpenScene = "";
        }
    }

    private IEnumerator LoadLevel(string sceneName)
    {
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone)
        {
            Debug.Log("Loading the Scene");
            yield return null;
        }
        CurrentOpenScene = sceneName;
    }

    private IEnumerator UnloadLevel(string sceneName)
    {
        var asyncLoadLevel = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncLoadLevel.isDone)
        {
            Debug.Log("Unloading the Scene");
            yield return null;
        }
    }
}
