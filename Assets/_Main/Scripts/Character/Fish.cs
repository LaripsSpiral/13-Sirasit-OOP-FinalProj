using System;
using UnityEngine;

namespace Main.Character
{
    public class Fish : BaseCharacter
    {
        public event Action OnDeath;
        public float GetSize() => transform.localScale.y;

        [Header("Eating")]
        [SerializeField]
        protected Transform mouthPos;

        [SerializeField]
        private float mouthSize;
        protected float mouthSizeSqr => mouthSize * GetSize();

        [SerializeField]
        private LayerMask eatingMask;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            EatingTarget();
        }

        public void EatingTarget()
        {
            if (mouthPos == default)
                return;

            var hit = Physics2D.CircleCast(mouthPos.position, mouthSizeSqr, transform.forward, mouthSizeSqr, eatingMask);
            if (hit == default || hit.collider.attachedRigidbody == default)
                return;

            if (!hit.collider.attachedRigidbody.TryGetComponent<Fish>(out var fishTarget))
                return;

            if (fishTarget == this)
                return;

            // Return, BiggerFish
            if (fishTarget.GetSize() > GetSize())
                return;

            Eat(fishTarget);
        }

        protected virtual void Eat(Fish targetFish)
        {
            targetFish.Eaten();
        }

        protected virtual void Eaten()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            if (mouthPos == default)
                return;

            Gizmos.DrawWireSphere(mouthPos.position, mouthSizeSqr);
        }
    }
}