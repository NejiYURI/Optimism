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

    public MainUIController GameUI;
    [SerializeField]
    private int DaysLeft = 3;


    [SerializeField]
    private float enthusiasm = 5f;
    public float enthusiasmMax = 10f;

    [Header("Time Setting")]
    public float RoundTime = 60f;
    [SerializeField]
    private float Cur_RoundTime;
    public Image TimerImg;

    [Header("Progress Setting")]
    public float MaxProgress = 10f;
    [SerializeField]
    private float CurrentProgress = 9.8f;

    [Header("Mini Game")]
    public GameObject MiniGameObject;
    public List<string> MiniGameScenes;

    public GameObject MiniGameBtn;
    public GameObject WorkBtn;
    public GameObject WorkStopBtn;
    private Coroutine GameCounter;
    private Coroutine DayTimeCounter;


    private void Start()
    {
        if (PlayerStatus.instance)
        {
            CurrentProgress = PlayerStatus.instance.CurrentProgress;
            MaxProgress = PlayerStatus.instance.MaxProgress;
            enthusiasm = PlayerStatus.instance.enthusiasm;
            enthusiasmMax = PlayerStatus.instance.enthusiasmMax;
            DaysLeft = PlayerStatus.instance.DaysLeft;
        }
        Cur_RoundTime = RoundTime;
        if (GameUI)
        {
            GameUI.SetDaysLeft(DaysLeft);
            GameUI.SetRatio(CurrentProgress / MaxProgress);
            GameUI.SetEnthusiasm(enthusiasm / enthusiasmMax);
            GameUI.StartPanel();
        }
        if (NormalCam && CodingCam)
        {
            NormalCam.Priority = 1;
            CodingCam.Priority = 0;
            GameCam.Priority = 0;
        }
        if (MainGameEvent.instance)
        {
            MainGameEvent.instance.GameStart.AddListener(GameStart);
            MainGameEvent.instance.CodeError.AddListener(CodeError);
            MainGameEvent.instance.CodeComplete.AddListener(CodeComplete);
        }
    }

    void GameStart()
    {
        DayTimeCounter = StartCoroutine(RoundTimer());
    }

    void CodeComplete()
    {
        CurrentProgress = Mathf.Clamp(CurrentProgress + 0.002f * enthusiasm, 0f, MaxProgress);
        if (GameUI) GameUI.SetProgressBar(CurrentProgress / MaxProgress);
        if (CurrentProgress >= MaxProgress)
        {
            dayOverFunc("");
            return;
        }
        enthusiasmChange(-0.2f);
    }

    void CodeError()
    {
        enthusiasmChange(-0.5f);
    }

    void enthusiasmChange(float _amount)
    {
        this.enthusiasm = Mathf.Clamp(this.enthusiasm + _amount, 0f, enthusiasmMax);
        if (GameUI) GameUI.SetEnthusiasm(enthusiasm / enthusiasmMax);
        if (this.enthusiasm <= 0)
        {
            dayOverFunc("You're tired");
        }
    }

    void dayOverFunc(string _content)
    {
        if (PlayerStatus.instance)
        {
            PlayerStatus.instance.CurrentProgress = CurrentProgress;
            PlayerStatus.instance.enthusiasm = this.enthusiasm <= 0 ? 5f : enthusiasm;
            PlayerStatus.instance.DaysLeft = DaysLeft - 1;
        }
        if (MainGameEvent.instance) MainGameEvent.instance.GameOver.Invoke();
        if (GameUI) GameUI.DayOver(_content);
        if (SceneManagerScript.instance) SceneManagerScript.instance.UnloadCurrentScene();
        if (GameCounter != null) StopCoroutine(GameCounter);
        if (DayTimeCounter != null) StopCoroutine(DayTimeCounter);
        if ((DaysLeft - 1) <= 0 || CurrentProgress >= MaxProgress) 
            StartCoroutine(LoadResultScene());
        else
            StartCoroutine(ReloadScene());
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
            if (GameCounter != null) StopCoroutine(GameCounter);
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
            if (MiniGameObject) MiniGameObject.SetActive(true);
            if (MainGameEvent.instance) MainGameEvent.instance.ActionSelect.Invoke("game");
            if (SceneManagerScript.instance && MiniGameScenes.Count>0) SceneManagerScript.instance.LoadSceneAdditive(MiniGameScenes[Random.Range(0, MiniGameScenes.Count)]);
            GameCounter = StartCoroutine(MiniGameCounter());
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
        dayOverFunc("Day's over");
    }

    IEnumerator MiniGameCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            enthusiasmChange(0.1f);
        }
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(3f);
        if (SceneManagerScript.instance) SceneManagerScript.instance.ReloadScene();
    }

    IEnumerator LoadResultScene()
    {
        yield return new WaitForSeconds(3f);
        if (SceneManagerScript.instance) SceneManagerScript.instance.LoadScene("ResultScene");
    }
}
