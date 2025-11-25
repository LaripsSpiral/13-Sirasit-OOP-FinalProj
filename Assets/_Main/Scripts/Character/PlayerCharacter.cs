using Main.Score;
using Main.Times;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace Main.Character
{
    public class PlayerCharacter : Fish
    {
        public event Action OnDeath;

        [SerializeField]
        private AudioClip takeDamageSound;

        // Update UI
        public event Action OnTakeDamage;
        public event Action<float> OnAte;

        [SerializeField]
        private Collider2D collider2D;

        [SerializeField, ReadOnly]
        private int currentHealth;
        public int CurrentHealth => currentHealth;

        [SerializeField]
        private int iFrameDuration = 1;

        [SerializeField]
        private ComboSystem comboSystem;

        [Header("Buff Settings")]
        [SerializeField, Tooltip("Additional buff multiplier per combo count. Final buff = baseBuff * (1 + comboCount * comboBuffMultiplier)")]
        private float comboBuffMultiplier = 0.1f;

        private ScoreSystem scoreSystem => ScoreSystem.Instance;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Move(moveDir);
        }

        private void Start()
        {
            // Auto-find combo system if not assigned in inspector
            if (comboSystem == null)
            {
                comboSystem = FindAnyObjectByType<ComboSystem>();
            }
        }

        public void Setup(int health)
        {
            currentHealth = health;
        }

        public void UpdateMoveInput(Vector2 moveInput)
        {
            moveDir = moveInput;
        }

        protected override void Eat(Fish targetFish)
        {
            int comboCount = comboSystem != null ? comboSystem.ComboCount : 0;

            base.Eat(targetFish);
            transform.localScale += Vector3.one * (targetFish.GetSize() / 30) * (comboCount / 5);
            OnAte?.Invoke(targetFish.GetSize());

            comboSystem?.AddCombo();
            scoreSystem?.AddScore(targetFish.GetSize() * 100 + (30 * comboCount));

            // Scale buff based on current combo count
            float baseBuff = targetFish.GetSize();
            float finalBuff = baseBuff * (1f + comboCount * comboBuffMultiplier);

            var newBuffTotal = AddTimedSpeedBuff(finalBuff, 5f);
            Debug.Log($"{this} applied speed buff (base:{baseBuff} combo:{comboCount} final:{finalBuff:F2}), current total SpeedBuff={newBuffTotal}");
        }
        protected override void Eaten()
        {
            TakeDamage();
        }

        private void TakeDamage()
        {
            // IFrame
            if (collider2D.enabled == false)
                return;

            // Do IFrame
            collider2D.enabled = false;

            var countDownEvent = new CountDownEvent(
                OnFinished: () => collider2D.enabled = true,
                CoolDownTime: iFrameDuration
            );

            CountDownTimer.Instance.AddCountDownEvent("PlayerIFrame", countDownEvent);

            currentHealth -= 1;
            Debug.Log($"{this} Taken Damage, left {currentHealth} Health");

            audioSource.PlayOneShot(takeDamageSound);
            OnTakeDamage?.Invoke();

            // Check Death
            if (currentHealth > 0)
                return;

            Debug.Log($"{this} have no Health left");
            Death();
        }

        protected override void Death()
        {
            gameObject.SetActive(false);
            OnDeath.Invoke();
        }
    }
}