using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeGameManger : MonoBehaviour
{
    public List<string> CodeList;
    private string CurrentTarget;
    public Transform CodeBox;
    public GameObject CodeObj;
    [Header("UI")]
    public TextMeshProUGUI TargetCode;
    public TextMeshProUGUI TypeCode;
    public TextMeshProUGUI RsltCode;
    void Start()
    {
        SetCode();
    }

    // Update is called once per frame
    void Update()
    {
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
                    //if (CodeBox && CodeObj)
                    //{
                    //    GameObject newCode = Instantiate(CodeObj, CodeBox);
                    //    if (newCode.GetComponent<TextMeshProUGUI>()) newCode.GetComponent<TextMeshProUGUI>().text = TargetCode.text;
                    //}
                    if (RsltCode)
                    {
                        RsltCode.text += "\n" + TargetCode.text;
                    }

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
