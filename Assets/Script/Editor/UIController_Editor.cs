using UnityEngine;
using UnityEditor;
namespace ToyPlanet
{
    [CustomEditor(typeof(UIController))]
    public class UIController_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UIController UIcontorl = (UIController)target;
            if (GUILayout.Button("Give me score~~"))
            {
                Debug.Log("Te ki bo");
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    UIcontorl.GetScore("Free Score!!", 87, UIcontorl.m_Camera.transform.position);
                }
            }


        }
    }
}
