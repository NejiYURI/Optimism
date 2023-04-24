using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AIDriving
{
    public class CarReplay : MonoBehaviour
    {
        private Rigidbody2D rb;

        private bool IsStart;

        private Queue<RecordData> RecordData;
        private RecordData startPos;

        public CarControl CarControl;

        public SpriteRenderer spriteRenderer;
        public Sprite ChangeSprite;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            RecordData = new Queue<RecordData>();
            if (CarControl)
            {
                startPos = CarControl.GetStartPos();
                RoundReady();
            }
            if (GameEventManager.instance)
            {
                GameEventManager.instance.RoundStart.AddListener(RoundStart);
                GameEventManager.instance.RoundFailed.AddListener(RoundFailed);
                GameEventManager.instance.RoundReady.AddListener(RoundReady);
            }
            if (spriteRenderer != null) spriteRenderer.sprite = ChangeSprite;
        }

        void RoundReady()
        {
            IsStart = false;
            if (rb != null)
            {
                this.rb.position = startPos.position;
                this.rb.rotation = startPos.Rotation;
            }
            if (CarControl) CarControl.GetRecord(out RecordData);
        }

        void RoundStart()
        {
            IsStart = true;
        }

        void RoundFailed()
        {
            IsStart = false;
        }

        private void Update()
        {
            if (rb == null || RecordData.Count <= 0 || !IsStart)
            {
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0;
                }
                return;
            }
            RecordData record = RecordData.Dequeue();
            rb.position = record.position;
            rb.rotation = record.Rotation;
            if (RecordData.Count <= 0) IsStart = false;
        }
    }
}
