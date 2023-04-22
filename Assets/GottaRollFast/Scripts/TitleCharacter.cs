using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TitleCharacter : MonoBehaviour
{
    public Rigidbody2D Head;

    public HingeJoint2D HeadJoint;

    public Rigidbody2D Sholder_1;

    public Rigidbody2D Sholder_2;
    private float HeadAngle;
    [SerializeField]
    private float HeadShakeCounter;
    private void Start()
    {
        HeadShakeCounter = 0;
        if (Head != null)
            HeadAngle = Head.rotation;
    }

    private void FixedUpdate()
    {
        if (Head != null)
        {
            Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 lookAt = mouseScreenPosition;

            float AngleRad = Mathf.Atan2(lookAt.y - Head.position.y, lookAt.x - Head.position.x);

            float AngleDeg = (180 / Mathf.PI) * AngleRad;

            Head.MoveRotation(AngleDeg);
            if (Mathf.Abs(HeadAngle - AngleDeg) >= 30)
            {
                HeadAngle = AngleDeg;
                HeadShakeCounter++;
                if (HeadShakeCounter >= 20f && GameSettingScript.instance != null && GameSettingScript.instance.Explosion)
                {
                    Debug.Log("Oops");
                    HeadJoint.enabled = false;
                    Head.velocity = Vector3.zero;
                    Head.AddForce(new Vector2(100f, 0f));
                    Head.gravityScale = 1f;
                    Head = null;
                }
                else if (HeadShakeCounter >= 20f)
                {
                    HeadShakeCounter = 0;
                }
            }
        }
    }

    public void ClickExplosion()
    {
        if (Sholder_1 != null) Sholder_1.AddTorque(-1000f);
        if (Sholder_2 != null) Sholder_2.AddTorque(1000f);
    }
}
