using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeGameManger : MonoBehaviour
{
    public string ActionId;
    public List<string> CodeList;
    private string CurrentTarget;
    public CodeBoxScript CodeBox;
    private bool CanType;
    [Header("UI")]
    public TextMeshProUGUI TargetCode;
    public TextMeshProUGUI TypeCode;
    public TextMeshProUGUI RsltCode;

    public AudioClip TypingSound;
    public AudioClip CompleteSound;
    public AudioClip MissSound;
    void Start()
    {
        SetCode();
        if (MainGameEvent.instance)
        {
            MainGameEvent.instance.ActionSelect.AddListener(ActionSelect);
            MainGameEvent.instance.GameOver.AddListener(GameOver);
        }
    }

    private void ActionSelect(string _id)
    {
        CanType = ActionId.Equals(_id);
    }

    void GameOver()
    {
        MainGameEvent.instance.ActionSelect.RemoveListener(ActionSelect);
        CanType = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanType) return;
        if (Input.anyKeyDown)
        {
            string inputstr = Input.inputString;
            if (inputstr.Length > 0)
                CodeCheck(inputstr);
        }
    }

    void CodeCheck(string input)
    {
        if (input.Length > 1 || input.Trim().Length<=0) return;
        if (CurrentTarget.Length > 0)
        {
            if (CurrentTarget[0] == input.ToCharArray()[0])
            {
                if (AudioController.instance) AudioController.instance.PlaySound(TypingSound,0.5f);
                TypeCode.text = TypeCode.text + input;
                if (CurrentTarget.Length == 1)
                {
                    if (MainGameEvent.instance) MainGameEvent.instance.CodeComplete.Invoke();
                    //if (RsltCode)
                    //{
                    //    RsltCode.text += "\n" + TargetCode.text;
                    //}
                    if (AudioController.instance) AudioController.instance.PlaySound(CompleteSound);
                    if (CodeBox) CodeBox.AddObj(TargetCode.text);
                    SetCode();
                }
                else
                {
                    CurrentTarget = CurrentTarget.Substring(1);
                    while (CurrentTarget[0].ToString() == " ")
                    {
                        TypeCode.text = TypeCode.text + " ";
                        CurrentTarget = CurrentTarget.Substring(1);
                    }
                }

            }
            else
            {
                if (AudioController.instance) AudioController.instance.PlaySound(MissSound);
                if (MainGameEvent.instance) MainGameEvent.instance.CodeError.Invoke();
            }
        }
    }

    void SetCode()
    {
        if (CodeList.Count > 0)
        {
            if (TargetCode && TypeCode)
            {
                CurrentTarget = CodeList[Random.Range(0, CodeList.Count)];
                TargetCode.text = CurrentTarget;
                TypeCode.text = "";
            }
        }
    }
}
