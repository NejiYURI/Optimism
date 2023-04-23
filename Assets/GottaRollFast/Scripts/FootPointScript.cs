using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GottaRollFast
{
    public class FootPointScript : MonoBehaviour
    {
        public PlayerControl playerControl;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (playerControl != null) playerControl.FootStep();
        }

    }
}
