using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Vector3 Offset;
    public Transform TargetObj;

    private void FixedUpdate()
    {
        if (TargetObj != null)
            this.transform.position = Vector3.Lerp(this.transform.position, TargetObj.position + Offset, 0.1f);
    }
}
