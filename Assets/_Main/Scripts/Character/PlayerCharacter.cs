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

        [SerializeField, ReadOnly]
        private int currentHealth;
        public int CurrentHealth => currentHealth;

        public void Setup(int health)
        {
            currentHealth = health;
        }

        public void UpdateMoveInput(Vector2 moveInput)
        {
            moveDir = moveInput;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            Debug.Log($"{this} Taken {damage} Damage, left {currentHealth} Health ");

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
    }
}