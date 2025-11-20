using Main.Times;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace Main.Character
{
    public class PlayerCharacter : Fish
    {
        public event Action OnDeath;

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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Move(moveDir);
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
            base.Eat(targetFish);
            OnAte?.Invoke(targetFish.GetSize());

            if (comboSystem != null)
            {
                comboSystem.AddCombo();
            }

            var newBuffTotal = AddTimedSpeedBuff(targetFish.GetSize(), 5f);
            Debug.Log($"{this} applied speed buff {targetFish.GetSize()}, current total SpeedBuff={newBuffTotal}");
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