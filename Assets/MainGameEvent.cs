using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainGameEvent : MonoBehaviour
{
    public static MainGameEvent instance;

    private void Awake()
    {
        if (MainGameEvent.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public UnityEvent<string> ActionSelect;

    public UnityEvent CodeComplete;
}
