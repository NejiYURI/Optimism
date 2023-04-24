using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    [Header("Progress Setting")]
    public float MaxProgress = 10f;
    public float CurrentProgress = 9.8f;

    [Header("Enthusiasm Setting")]
    public float enthusiasm;
    public float enthusiasmMax;

    public int DaysLeft;
    private void Awake()
    {
        if (PlayerStatus.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
