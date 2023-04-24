using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
namespace AIDriving
{
    public class GameManager : MonoBehaviour
    {
        public GameObject Player;
        public GameObject Objective;

        public GameObject GameOverPanel;

        public SpriteRenderer BlackScreen;

        public List<Transform> SpawnPos;

        public float RoundTime = 5;

        public AudioClip FailedSound;
        public AudioClip RewindSound;

        public Image TimerImg;
        public TextMeshProUGUI LifeText;
        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI GameOverText;
        public TextMeshProUGUI FinalScoreText;

        [SerializeField]
        private int score;

        private List<int> RandIndex;
        private int RandCount;

        private Coroutine CountDownTimer;

        public int LifeSet;

        private bool IsGameOver;

        void Start()
        {

            if (GameEventManager.instance)
            {
                GameEventManager.instance.RoundClear.AddListener(RoundEnd);
                GameEventManager.instance.RoundFailed.AddListener(RoundFailed);
            }
            RandIndex = new List<int>();
            RandCount = SpawnPos.Count;
            for (int i = 0; i < SpawnPos.Count; i++)
            {
                RandIndex.Add(i);
            }
            LifeText.text = " x " + LifeSet;
            score = 0;
            ScoreText.text = score.ToString();
            SpawnObj();
            StartCoroutine(StartTimer());
            GameOverPanel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (IsGameOver && Input.GetKey(KeyCode.R))
            {
                IsGameOver = false;
                Retry();
            }
        }

        void RoundEnd()
        {
            StopCoroutine(CountDownTimer);
            TimerImg.fillAmount = 0f;
            score += 500;
            score += Mathf.RoundToInt(RoundTime * 50);
            ScoreText.text = score.ToString();

            if (RandCount > 0)
            {

                SpawnObj();
                StartCoroutine(StartTimer());
            }
            else
            {
                SetGameOverContent("Wut?");
                GameOverPanel.SetActive(true);
            }

        }

        void RoundFailed()
        {
            TimerImg.fillAmount = 0f;
            if (CountDownTimer != null) StopCoroutine(CountDownTimer);
            LifeSet--;
            LifeText.text = " x " + LifeSet;
            if (AudioControl.instance) AudioControl.instance.PlaySound(FailedSound, 1f);
            if (LifeSet <= 0)
            {
                SetGameOverContent("GameOver");
                GameOverPanel.SetActive(true);
                IsGameOver = true;
            }
            else
            {
                StartCoroutine(StartTimer());
            }

        }

        int GetRand()
        {
            if (RandCount > 0)
            {
                int getIndex = Random.Range(0, RandCount);
                int Rslt = RandIndex[getIndex];
                RandIndex.RemoveAt(getIndex);
                RandIndex.Add(Rslt);
                RandCount--;
                return Rslt;
            }
            return -1;
        }

        IEnumerator StartTimer()
        {
            if (AudioControl.instance) AudioControl.instance.PlaySound(RewindSound, 0.5f);
            BlackScreen.enabled = true;
            BlackScreen.maskInteraction = SpriteMaskInteraction.None;
            yield return new WaitForSeconds(1f);
            BlackScreen.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            if (GameEventManager.instance) GameEventManager.instance.RoundReady.Invoke();
            yield return new WaitForSeconds(1f);
            BlackScreen.enabled = false;
            CountDownTimer = StartCoroutine(CountDown());
            if (GameEventManager.instance) GameEventManager.instance.RoundStart.Invoke();
        }
        IEnumerator CountDown()
        {
            RoundTime = 5;
            TimerImg.fillAmount = RoundTime / 5f;
            while (RoundTime > 0)
            {
                RoundTime -= Time.deltaTime;
                TimerImg.fillAmount = RoundTime / 5f;
                yield return null;
            }
            GameEventManager.instance.RoundFailed.Invoke();
        }

        void SpawnObj()
        {
            int tmp = GetRand();
            GameObject playerObj = Instantiate(Player, SpawnPos[tmp].position, SpawnPos[tmp].rotation);
            SceneManager.MoveGameObjectToScene(playerObj, SceneManager.GetSceneByName("AIDriving"));
            int objInd = (tmp + SpawnPos.Count / 2) % SpawnPos.Count;
            RandCount--;
            RandIndex.Remove(objInd);
            RandIndex.Add(objInd);
            GameObject Objobj= Instantiate(Objective, SpawnPos[objInd].position, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(Objobj, SceneManager.GetSceneByName("AIDriving"));
        }

        void SetGameOverContent(string labelContent)
        {
            GameOverText.text = labelContent;
            FinalScoreText.text = "Score: " + score;
        }

        public void Retry()
        {
            if (SceneManagerScript.instance)
            {
                SceneManagerScript.instance.LoadSceneAdditive("AIDriving");
                SceneManagerScript.instance.UnloadScene("AIDriving");
            }
        }

        public void ToTitle()
        {
            SceneManager.LoadScene("Title");
        }
    }
}

