using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CodeTextScript : MonoBehaviour
{
    public TextMeshProUGUI TargetText;

    private void Start()
    {
        if (MainGameEvent.instance) MainGameEvent.instance.CodeError.AddListener(CodeError);
    }

    void CodeError()
    {
        if (TargetText)
            StartCoroutine(MissAction());
    }

    IEnumerator MissAction()
    {
        Color OriginColor = TargetText.color;
        Color ChangeColor = Color.red;
        ChangeColor.a = OriginColor.a;
       
        TargetText.color = ChangeColor;
        yield return new WaitForSeconds(0.1f);
        TargetText.color = OriginColor;
    }
}
