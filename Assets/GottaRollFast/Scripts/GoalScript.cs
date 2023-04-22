using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.GetComponent<PlayerControl>() != null)
        {
            Debug.Log("PlayerIn");
            if (!collision.gameObject.GetComponent<PlayerControl>().IsDead)
            {
                if (GameEventManager.instance != null) GameEventManager.instance.StageClear.Invoke();
            }
        }
    }
}
