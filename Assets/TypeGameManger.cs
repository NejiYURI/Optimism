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
    void Start()
    {
        SetCode();
        if (MainGameEvent.instance) MainGameEvent.instance.ActionSelect.AddListener(ActionSelect);
    }

    private void ActionSelect(string _id)
    {
        CanType = ActionId.Equals(_id);
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
        if (input.Length > 1) return;
        if (CurrentTarget.Length > 0)
        {
            if (CurrentTarget[0] == input.ToCharArray()[0])
            {

                TypeCode.text = TypeCode.text + input;
                if (CurrentTarget.Length == 1)
                {

                    //if (RsltCode)
                    //{
                    //    RsltCode.text += "\n" + TargetCode.text;
                    //}
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
