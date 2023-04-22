using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdControl : MonoBehaviour
{

    private Rigidbody2D rg;

    public float FlyForce;

    [SerializeField]
    private Vector2 Direction;

    // Start is called before the first frame update
    void Start()
    {
        this.rg = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FlyInput(InputAction.CallbackContext context)
    {
        if (context.performed) this.rg.AddForce(new Vector2(Direction.x,1f)* FlyForce);
    }

    public void GetDirection(InputAction.CallbackContext context)
    {
        if (context.performed)
            Direction = context.ReadValue<Vector2>();
        else
            Direction = Vector2.zero;
    }
}
