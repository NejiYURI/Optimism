using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{

    public CinemachineVirtualCamera NormalCam;
    public CinemachineVirtualCamera CodingCam;
    public CinemachineVirtualCamera GameCam;
    [Header("Time Setting")]
    public float RoundTime = 60f;
    [SerializeField]
    private float Cur_RoundTime;
    public Image TimerImg;

    [Header("Progress Setting")]
    public float MaxProgress = 60f;
    public float CurrentProgress = 50f;
    public Image ProgressBar;

    [Header("Mini Game")]
    public GameObject MiniGameObject;

    public GameObject MiniGameBtn;
    public GameObject WorkBtn;
    public GameObject WorkStopBtn;


    private void Start()
    {
        Cur_RoundTime = RoundTime;
        if (ProgressBar) ProgressBar.fillAmount = CurrentProgress / MaxProgress;
        if (NormalCam && CodingCam)
        {
            NormalCam.Priority = 1;
            CodingCam.Priority = 0;
            GameCam.Priority = 0;
        }

        StartCoroutine(RoundTimer());
    }

    public void WorkButton()
    {
        if (NormalCam && CodingCam)
        {
            NormalCam.Priority = 0;
            CodingCam.Priority = 1;
            if (WorkBtn) WorkBtn.SetActive(false);
            if (WorkStopBtn) WorkStopBtn.SetActive(true);
            if (MiniGameBtn) MiniGameBtn.SetActive(false);
            if (MainGameEvent.instance) MainGameEvent.instance.ActionSelect.Invoke("work");
        }
    }

    public void ReturnNormalBtn()
    {
        if (NormalCam && CodingCam)
        {
            NormalCam.Priority = 1;
            CodingCam.Priority = 0;
            GameCam.Priority = 0;
            if (WorkBtn) WorkBtn.SetActive(true);
            if (WorkStopBtn) WorkStopBtn.SetActive(false);
            if (MiniGameBtn) MiniGameBtn.SetActive(true);
            if (MiniGameObject) MiniGameObject.SetActive(false);
            if (MainGameEvent.instance) MainGameEvent.instance.ActionSelect.Invoke("normal");
            if (SceneManagerScript.instance) SceneManagerScript.instance.UnloadCurrentScene();
        }
    }

    public void GameBtn()
    {
        if (NormalCam && GameCam)
        {
            NormalCam.Priority = 0;
            GameCam.Priority = 1;
            if (WorkBtn) WorkBtn.SetActive(false);
            if (MiniGameBtn) MiniGameBtn.SetActive(false);
            if (MiniGameObject)  MiniGameObject.SetActive(true);
            if (MainGameEvent.instance) MainGameEvent.instance.ActionSelect.Invoke("game");
            if (SceneManagerScript.instance) SceneManagerScript.instance.LoadSceneAdditive("Stage1");
        }
    }



    IEnumerator RoundTimer()
    {
        while (Cur_RoundTime > 0)
        {
            Cur_RoundTime -= Time.deltaTime;
            if (TimerImg) TimerImg.fillAmount = Cur_RoundTime / RoundTime;
            yield return null;
        }
    }
}
