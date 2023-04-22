using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterColliderScript : MonoBehaviour
{
    public PlayerControl playerControl;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerControl != null) playerControl.PlayerCrash();
    }
}
