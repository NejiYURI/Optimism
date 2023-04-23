using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeBoxScript : MonoBehaviour
{
    public GameObject CodeObj;
    public List<GameObject> childrenObj;

    public int MaxObj=6;

    private void Start()
    {
        childrenObj = new List<GameObject>();
    }

    public void AddObj(string _content)
    {

        if (CodeObj)
        {
            if (childrenObj.Count + 1 > MaxObj)
            {
                Destroy(childrenObj[0]);
                childrenObj.RemoveAt(0);
            }
            GameObject newCode = Instantiate(CodeObj, this.transform);
            if (newCode.GetComponent<TextMeshProUGUI>()) newCode.GetComponent<TextMeshProUGUI>().text = _content;
            childrenObj.Add(newCode);
        }
    }
}
