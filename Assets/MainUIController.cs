using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public Image ProgressBar;
    public TextMeshProUGUI daysLeftTxt;

    public TextMeshProUGUI DayOverText;

    public List<Sprite> enthusiasmStates;
    public Image enthusiasmImg;

    public bool IsResult;
    public Animator animator;

    private float ratio;
    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    public void SetDaysLeft(int _days)
    {
        if (daysLeftTxt)
        {
            daysLeftTxt.text = "<#" + ColorUtility.ToHtmlStringRGB(Color.yellow) + ">" + _days.ToString() + "</color> days left";
        }
    }

    public void SetEnthusiasm(float _ratio)
    {

        for (int i = 0; i < enthusiasmStates.Count; i++)
        {
            if (((float)i / (float)enthusiasmStates.Count) >= _ratio)
            {
                break;
            }
            else
            {
                if (enthusiasmImg)
                {
                    enthusiasmImg.sprite = enthusiasmStates[i];
                    StartCoroutine(EmojiChange());
                }
            }
        }
    }

    public void StartPanel()
    {
        if (animator) animator.Play("StartPanel_start");
    }
    public void SetRatio(float _ratio)
    {
        ratio = _ratio;
    }

    public void SetProgressBar(float _ratio)
    {
        SetRatio(_ratio);
        if (ProgressBar) ProgressBar.fillAmount = ratio;
    }

    public void ProgressMove()
    {
        StartCoroutine(MoveTimer());
    }

    public void DayOver(string _ShowTxt)
    {
        if (animator) animator.Play("EndPanel");
        if (DayOverText) DayOverText.text = _ShowTxt;
    }

    IEnumerator MoveTimer()
    {
        ProgressBar.fillAmount = 0;
        float timer = 0;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            ProgressBar.fillAmount = Mathf.Lerp(0, ratio, timer / 1.5f);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        SetProgressBar(ratio);
        if (animator && !IsResult) animator.Play("StartPanel_end");
        if (MainGameEvent.instance) MainGameEvent.instance.GameStart.Invoke();
    }
    IEnumerator EmojiChange()
    {
        enthusiasmImg.rectTransform.LeanScale(new Vector2(1.2f, 1.2f), 0.1f);
        yield return new WaitForSeconds(0.1f);
        enthusiasmImg.rectTransform.LeanScale(Vector2.one, 0.1f);
        yield return new WaitForSeconds(0.1f);
    }
}
