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
        public event Action OnAte;

        [SerializeField]
        private Collider2D collider2D;

        [SerializeField, ReadOnly]
        private int currentHealth;
        public int CurrentHealth => currentHealth;

        private float iFrameDuration = 2;
        private float iFrameElapsedTime;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Move(moveDir);

            UpdateCooldownIFrame();
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
            OnAte?.Invoke();
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
            iFrameElapsedTime = iFrameDuration;
            collider2D.enabled = false;

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
            this.enabled = false;
            OnDeath.Invoke();
        }

        private void UpdateCooldownIFrame()
        {
            if (iFrameElapsedTime > 0)
            {
                iFrameElapsedTime -= Time.deltaTime;
                return;
            }

            // Reset IFrame
            iFrameElapsedTime = 0;
            collider2D.enabled = true;
        }
    }
}