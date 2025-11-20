using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Times
{
    public class CountDownTimer : MonoBehaviour
    {
        public static CountDownTimer Instance;

        private Dictionary<string, CountDownEvent> countDownByID = new();

        private void Awake()
        {
            Instance = this;
        }

        private void FixedUpdate()
        {
            foreach (var cdKeyPair in countDownByID)
            {
                UpdateCooldown(cdKeyPair.Key, cdKeyPair.Value);
            }
        }

        public void AddCountDownEvent(string id, CountDownEvent cdEvent)
        {
            // Already had event
            if (countDownByID.ContainsKey(id))
            {
                Debug.Log($"Replaced CountDown ID:{id}");
                countDownByID[id] = cdEvent;
                return;
            }

            Debug.Log($"Added CountDown ID:{id}");
            countDownByID.Add(id, cdEvent);
        }

        private void UpdateCooldown(string id, CountDownEvent cdEvent)
        {
            if (cdEvent.IsFinished)
                return;

            // If there's still time left, decrement and call OnTick with remaining ratio
            if (cdEvent.ElapsedTime > 0)
            {
                cdEvent.ElapsedTime -= Time.fixedDeltaTime;
                float ratio = cdEvent.CoolDownTime > 0 ? Mathf.Clamp01(cdEvent.ElapsedTime / cdEvent.CoolDownTime) : 0f;
                cdEvent.OnTick?.Invoke(ratio);

                // Not finished yet
                return;
            }

            // Mark finished and invoke final callbacks
            Debug.Log($"Finished CountDown ID:{id}");
            cdEvent.IsFinished = true;
            cdEvent.OnTick?.Invoke(0f);
            cdEvent.OnFinished?.Invoke();
        }
    }

    public class CountDownEvent
    {
        public Action OnFinished;

        // Called each update with remaining ratio (0..1)
        public Action<float> OnTick;

        public bool IsFinished;
        public float CoolDownTime;
        public float ElapsedTime;

        public CountDownEvent(Action OnFinished, float CoolDownTime)
        {
            this.IsFinished = false;

            this.OnFinished = OnFinished;
            this.CoolDownTime = CoolDownTime;
            this.ElapsedTime = CoolDownTime;
            this.OnTick = null;
        }
    }
}