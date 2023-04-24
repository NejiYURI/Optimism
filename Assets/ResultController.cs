using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultController : MonoBehaviour
{
    public MainUIController GameUI;
    [SerializeField]
    private int DaysLeft = 0;
    public float MaxProgress = 10f;
    [SerializeField]
    private float CurrentProgress = 9.8f;

    public TextMeshProUGUI Result;

    public GameObject ReturnBtn;
    void Start()
    {
        if (PlayerStatus.instance)
        {
            CurrentProgress = PlayerStatus.instance.CurrentProgress;
            MaxProgress = PlayerStatus.instance.MaxProgress;
            DaysLeft = PlayerStatus.instance.DaysLeft;
            Destroy(PlayerStatus.instance.gameObject);
        }
        if (GameUI)
        {
            GameUI.SetDaysLeft(DaysLeft);
            GameUI.SetRatio(CurrentProgress/ MaxProgress);
            GameUI.StartPanel();
        }
        if (MainGameEvent.instance)
        {
            MainGameEvent.instance.GameStart.AddListener(ShowResult);
        }
        if (Result) Result.text = "";
    }

    void ShowResult()
    {
        if (ReturnBtn) ReturnBtn.SetActive(true);
        if (CurrentProgress / MaxProgress >= 1)
        {
            if (Result) Result.text = "You did it!";
        }
        else
        {
            if (Result) Result.text = "Work failed successfully!";
        }
    }

    public void ReturnToTitle()
    {
        if (SceneManagerScript.instance) SceneManagerScript.instance.LoadScene("Title");
    }
}
