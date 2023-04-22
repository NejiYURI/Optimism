using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public CameraFollowScript cameraFollow;

    public Transform StarPos;

    public GameObject PlayerObj;

    public GameObject DeathPanel;

    public GameObject ClearPanel;

    public AudioClip GameClearSound;

    private PlayerInputAction inputActions;

    private bool IsPlayerDeath;
    private bool IsStageClear;

    private void Awake()
    {
        instance = this;
        inputActions = new PlayerInputAction();

    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }


    private void Start()
    {
        IsPlayerDeath = true;
        SpawnPlayer();
        inputActions.PlayerInput.Retry.performed += _ => SpawnPlayer();
        inputActions.PlayerInput.Confirm.performed += _ => StageClearConfirm();
        this.DeathPanel.SetActive(false);
        this.ClearPanel.SetActive(false);
        if (GameEventManager.instance != null)
        {
            GameEventManager.instance.StageClear.AddListener(StageClear);
        }
    }

    void SpawnPlayer()
    {
        if (IsStageClear)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        if (!IsPlayerDeath) return;
        IsPlayerDeath = false;
        IsStageClear = false;
        this.DeathPanel.SetActive(false);
        this.ClearPanel.SetActive(false);
        GameObject obj = Instantiate(PlayerObj, StarPos.position, Quaternion.identity);
        cameraFollow.TargetObj = obj.transform;
    }

    public void GameOver()
    {
        if (IsStageClear) return;
        IsPlayerDeath = true;
        cameraFollow.TargetObj = null;
        this.DeathPanel.SetActive(true);
    }

    public void StageClear()
    {
        IsStageClear = true;
        if (AudioController.instance != null) AudioController.instance.PlaySound(GameClearSound, 0.1f);
        this.ClearPanel.SetActive(true);
    }

    public void StageClearConfirm()
    {
        if (!IsStageClear) return;
        ReturnTitle();
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
