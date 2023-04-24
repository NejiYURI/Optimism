using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleControl : MonoBehaviour
{
    private PlayerInput inputActions;

    private Rigidbody2D rb;

    public float MoveSpeed = 1f;
    public float RotateSpeed = 1f;

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Awake()
    {
        inputActions = new PlayerInput();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        SetMovement(inputActions.CarControl.Control.ReadValue<Vector2>().normalized);
    }

    void SetMovement(Vector2 Movement)
    {
        if (rb == null) return;
        rb.AddForce(this.transform.up * Mathf.Round(Movement.y) * MoveSpeed);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -100f, 100f), Mathf.Clamp(rb.velocity.y, -100f, 100f));
        if (AudioControl.instance) AudioControl.instance.SetCarSound(rb.velocity.magnitude / 6f);
        rb.AddTorque(-1f * Mathf.Round(Movement.x) * RotateSpeed);
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -100f, 100f);
    }
}
