using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace AIDriving
{
    public class RecordData
    {
        public RecordData(Vector2 _position, float _Rotation)
        {
            this.position = _position;
            this.Rotation = _Rotation;
        }
        public Vector2 position;
        public float Rotation;
    }

    public class CarControl : MonoBehaviour
    {

        private Rigidbody2D rb;

        public float MoveSpeed = 1f;
        public float RotateSpeed = 1f;

        public CarReplay carReplay;

        public SpriteMask mask;
        private Queue<RecordData> RecordData;

        private RecordData OriginData;

        private PlayerInput inputActions;

        private bool IsStart;

        private bool IsOver;

        private void Awake()
        {
            inputActions = new PlayerInput();
        }
        private void OnEnable()
        {

        }
        private void OnDisable()
        {
            inputActions.Disable();
        }
        private void Start()
        {
            carReplay.enabled = false;
            inputActions.Disable();
            rb = GetComponent<Rigidbody2D>();
            if (GameEventManager.instance)
            {
                GameEventManager.instance.RoundStart.AddListener(RoundStart);
                GameEventManager.instance.RoundFailed.AddListener(ResetPlayer);
            }
            RecordData = new Queue<RecordData>();
            if (rb) OriginData = new RecordData(rb.position, rb.rotation);
        }

        void RoundStart()
        {
            IsStart = true;
            inputActions.Enable();
        }

        private void Update()
        {
            if (!IsStart) return;
            RecordData.Enqueue(new RecordData(rb.position, rb.rotation));
        }

        private void FixedUpdate()
        {
            if (!IsStart) return;
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

        public void GetRecord(out Queue<RecordData> recordDatas)
        {
            recordDatas = new Queue<RecordData>();
            foreach (var item in RecordData)
            {
                recordDatas.Enqueue(item);
            }
        }

        public RecordData GetStartPos()
        {
            return OriginData;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Objective") && !IsOver)
            {
                Destroy(collision.gameObject);
                if (GameEventManager.instance)
                {
                    GameEventManager.instance.RoundStart.RemoveListener(RoundStart);
                    GameEventManager.instance.RoundFailed.RemoveListener(ResetPlayer);
                    GameEventManager.instance.RoundClear.Invoke();
                }
                if (AudioControl.instance) AudioControl.instance.SetCarSound(0f);
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                carReplay.enabled = true;
                inputActions.Disable();
                if (GetComponent<BoxCollider2D>()) GetComponent<BoxCollider2D>().isTrigger = true;
                IsOver = true;
                this.enabled = false;
                mask.enabled = false;
            }

            if (collision.tag.Equals("Player") && !IsOver && IsStart)
            {
                IsStart = false;
                if (GameEventManager.instance)
                {
                    GameEventManager.instance.RoundFailed.Invoke();
                }
            }
        }

        void ResetPlayer()
        {
            if (AudioControl.instance) AudioControl.instance.SetCarSound(0f);
            IsStart = false;
            RecordData = new Queue<RecordData>();
            rb.position = OriginData.position;
            rb.rotation = OriginData.Rotation;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
    }
}
