using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    public Rigidbody2D PlayerLeg;

    public Rigidbody2D Sholder_1;

    public Rigidbody2D Sholder_2;



    private Rigidbody2D rg;

    public float MaxSpeed;
    [SerializeField]
    private float CurSpeed;
    [SerializeField]
    private float BalanceValue;
    public float BalanceSpeed;

    public float LegSpeedLimit;

    public float JumpForce;



    public float ExplosionForce;

    [SerializeField]
    private string StepStr;

    private bool CanJump;

    public bool IsDead;

    public bool CanFootStep;

    public AudioClip DeathSound;
    public AudioClip JumpSound;
    public AudioClip FootStepSound;

    private PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    void Start()
    {
        CanJump = true;
        CanFootStep = true;
        this.rg = GetComponent<Rigidbody2D>();
        inputActions.PlayerInput.Move_L.performed += _ => LeftPress();
        inputActions.PlayerInput.Move_R.performed += _ => RightPress();
        inputActions.PlayerInput.Jump.performed += _ => JumpFunc();
        if (GameEventManager.instance != null)
        {
            GameEventManager.instance.StageClear.AddListener(StageClear);
        }
    }

    // Update is called once per frame
    void Update()
    {

        BalanceValue = inputActions.PlayerInput.Balance.ReadValue<float>();

        CurSpeed = Mathf.Clamp(CurSpeed - Time.deltaTime * 20f, 0f, MaxSpeed);
        //if (GameEventManager.instance != null) GameEventManager.instance.ChargeLeg.Invoke(Input.GetKey(KeyCode.Space));
    }

    private void FixedUpdate()
    {
        PlayerLeg.angularVelocity = Mathf.Clamp(PlayerLeg.angularVelocity, -LegSpeedLimit, LegSpeedLimit);
        if (BalanceValue != 0)
        {
            this.rg.AddTorque(BalanceValue * BalanceSpeed * Time.deltaTime);
        }
    }

    void LeftPress()
    {
        if (!StepStr.Equals("L"))
        {
            CurSpeed = Mathf.Clamp(CurSpeed + MaxSpeed / 3f, 0f, MaxSpeed);
            PlayerLeg.AddTorque(-1f * CurSpeed);
            StepStr = "L";
        }
        else
        {
            CurSpeed = Mathf.Clamp(CurSpeed - MaxSpeed / 5f, 0f, MaxSpeed);
            PlayerLeg.angularVelocity = Mathf.Clamp(PlayerLeg.angularVelocity + LegSpeedLimit / 10f, -LegSpeedLimit, 0);
        }
    }

    void RightPress()
    {
        if (!StepStr.Equals("R"))
        {
            CurSpeed = Mathf.Clamp(CurSpeed + MaxSpeed / 3f, 0f, MaxSpeed);
            PlayerLeg.AddTorque(-1f * CurSpeed);
            StepStr = "R";
        }
        else
        {
            CurSpeed = Mathf.Clamp(CurSpeed - MaxSpeed / 5f, 0f, MaxSpeed);
            PlayerLeg.angularVelocity = Mathf.Clamp(PlayerLeg.angularVelocity + LegSpeedLimit / 10f, -LegSpeedLimit, 0);
        }
    }

    void JumpFunc()
    {
        if (!CanJump) return;
        StartCoroutine(JumpCoolDown());
        if (Sholder_1 != null) Sholder_1.AddTorque(-1000f);
        if (Sholder_2 != null) Sholder_2.AddTorque(1000f);
        if (AudioController.instance != null) AudioController.instance.PlaySound(JumpSound, 0.8f);
        this.rg.AddForce(new Vector2(0, JumpForce + -1 * this.rg.velocity.y));
    }



    public void PlayerCrash()
    {
        if (IsDead) return;
        if (AudioController.instance != null) AudioController.instance.PlaySound(DeathSound, 0.4f);
        IsDead = true;
        inputActions.Disable();
        if (GameSettingScript.instance != null && GameSettingScript.instance.Explosion)
        {
            DeathExplosion();
        }
        if (MainGameManager.instance != null) MainGameManager.instance.GameOver();
    }

    void DeathExplosion()
    {
        var hinge2DList = this.GetComponentsInChildren<HingeJoint2D>();
        foreach (var hinge in hinge2DList)
        {
            hinge.enabled = false;
        }

        var Rigidbodys = this.GetComponentsInChildren<Rigidbody2D>();

        foreach (var _rg in Rigidbodys)
        {
            _rg.AddForce(new Vector2(Random.Range(-5f, 5f) * ExplosionForce, Random.Range(3, 10f) * ExplosionForce));
            _rg.AddTorque(Random.Range(-5f, 5f) * ExplosionForce);
        }

        this.rg.AddForce(new Vector2(Random.Range(-5f, 5f) * ExplosionForce, Random.Range(3, 10f) * ExplosionForce));
        this.rg.AddTorque(Random.Range(-5f, 5f) * ExplosionForce);
    }

    public void StageClear()
    {
        inputActions.Disable();
    }

    public void FootStep()
    {
        if (IsDead || !CanFootStep) return;
        if (AudioController.instance != null) AudioController.instance.PlaySound(FootStepSound, 0.05f);
        StartCoroutine(FootStepCooldown());
    }

    IEnumerator FootStepCooldown()
    {
        CanFootStep = false;
        yield return new WaitForSeconds(0.3f);
        CanFootStep = true;
    }

    IEnumerator JumpCoolDown()
    {
        CanJump = false;
        yield return new WaitForSeconds(1f);
        CanJump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerCrash();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Trap"))
            PlayerCrash();
    }
}
