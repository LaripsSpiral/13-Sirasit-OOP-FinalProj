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
                UpdateCooldownIFrame(cdKeyPair.Key, cdKeyPair.Value);
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

        private void UpdateCooldownIFrame(string id, CountDownEvent cdEvent)
        {
            if (cdEvent.ElapsedTime > 0)
            {
                cdEvent.ElapsedTime -= Time.fixedDeltaTime;
                return;
            }

            if (cdEvent.IsFinished)
                return;

            Debug.Log($"Finished CountDown ID:{id}");
            cdEvent.IsFinished = true;
            cdEvent.OnFinished?.Invoke();
        }
    }

    public class CountDownEvent
    {
        public Action OnFinished;

        public bool IsFinished;
        public float CoolDownTime;
        public float ElapsedTime;

        public CountDownEvent(Action OnFinished, float CoolDownTime)
        {
            this.IsFinished = false;

            this.OnFinished = OnFinished;
            this.CoolDownTime = CoolDownTime;
            this.ElapsedTime = CoolDownTime;
        }
    }
}