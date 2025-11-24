using NaughtyAttributes;
using UnityEngine;
using System;
using Main.Effects;
using Main.Times;

namespace Main.Character
{
    public abstract class BaseCharacter : MonoBehaviour
    {
        [SerializeField]
        protected float speed = 10;
        public float Speed => speed;

        private EffectBuff effectBuff = new EffectBuff();

        public float CurrentSpeedBuff => effectBuff.SpeedBuff;
        public event Action<float> OnSpeedBuffChanged;
        public event Action<float> OnSpeedBuffProgress;

        public float AddSpeedBuff(float value)
        {
            var result = effectBuff.AddSpeedBuff(value);
            OnSpeedBuffChanged?.Invoke(effectBuff.SpeedBuff);
            return result;
        }

        public float RemoveSpeedBuff(float value)
        {
            var result = effectBuff.RemoveSpeedBuff(value);
            OnSpeedBuffChanged?.Invoke(effectBuff.SpeedBuff);
            return result;
        }

        public float AddTimedSpeedBuff(float value, float duration)
        {
            var result = AddSpeedBuff(value);

            if (CountDownTimer.Instance == null)
            {
                Debug.LogWarning("CountDownTimer instance not found. Timed buff will not be removed automatically.");
                return result;
            }

            // Why id ? 
            // To ensure each timed buff is uniquely identified, preventing conflicts or overwrites (six - seven)
            string id = $"SpeedBuff_{GetInstanceID()}_{Time.time}";
            var cdEvent = new CountDownEvent(
                OnFinished: () => RemoveSpeedBuff(value),
                CoolDownTime: duration
            );
            // Forward per-frame progress to listeners (remaining ratio)
            cdEvent.OnTick = (ratio) => OnSpeedBuffProgress?.Invoke(ratio);
            CountDownTimer.Instance.AddCountDownEvent(id, cdEvent);

            return result;
        }

        [SerializeField, ReadOnly]
        protected Vector2 moveDir;

        [SerializeField, ReadOnly]
        protected Rigidbody2D rb2d;
        public Rigidbody2D Rb2d => rb2d;

        private void OnValidate()
        {
            rb2d ??= GetComponent<Rigidbody2D>();
        }

        protected virtual void FixedUpdate()
        {
            RotateAlongVelocity();
        }

        protected void Move(Vector2 moveDir, ForceMode2D forceMode2D = ForceMode2D.Force, float multiplier = 1)
        {
            float effectiveSpeed = speed + effectBuff.SpeedBuff;
            Rb2d.AddForce(effectiveSpeed * multiplier * Time.fixedDeltaTime * moveDir, forceMode2D);
        }

        private void RotateAlongVelocity()
        {
            Vector2 dir = Rb2d.linearVelocity;

            if (dir.sqrMagnitude < 0.001f)
                return;

            float rotAngleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float finalRotZ;
            float rotAngleY;

            if (dir.x < 0)
            {
                rotAngleY = 180f;
                finalRotZ = 180f - rotAngleZ;
            }
            else
            {
                rotAngleY = 0f;
                finalRotZ = rotAngleZ;
            }

            transform.localEulerAngles = new Vector3(0f, rotAngleY, finalRotZ);
        }
        public float SetSpeed(float value)
        {
            speed = value;
            return speed;
        }

        protected virtual void Death()
        {
            Destroy(gameObject);
        }
    }
}