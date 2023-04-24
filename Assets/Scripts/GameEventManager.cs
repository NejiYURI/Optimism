using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace AIDriving
{
    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager instance;

        private void Awake()
        {
            instance = this;
        }

        public UnityEvent RoundReady;

        public UnityEvent RoundStart;

        public UnityEvent RoundFailed;

        public UnityEvent RoundClear;
    }
}
