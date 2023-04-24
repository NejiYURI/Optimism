using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour
{
    public GameObject CreditPanel;

    private void Start()
    {
        if (CreditPanel) CreditPanel.transform.LeanScale(Vector2.zero, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if (CreditPanel) CreditPanel.transform.LeanScale(Vector2.one, 0.1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if (CreditPanel) CreditPanel.transform.LeanScale(Vector2.zero, 0.1f);
        }
    }
}
