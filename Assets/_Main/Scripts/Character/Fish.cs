using UnityEngine;

namespace Main.Character
{
    public class Fish : BaseCharacter
    {
        [Header("Eating")]
        [SerializeField]
        private Transform mouthPos;

        [SerializeField]
        private float mouthSize;

        [SerializeField]
        private LayerMask eatingMask;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            EatingTarget();
        }

        public float GetSize() => transform.localScale.x;

        public void EatingTarget()
        {
            if (mouthSize == default)
                return;

            var hit = Physics2D.CircleCast(mouthPos.position, mouthSize, transform.forward, mouthSize, eatingMask);
            if (hit == default)
                return;

            if (!hit.collider.TryGetComponent<Fish>(out var fishTarget))
                return;

            if (fishTarget == this)
                return;

            Eat(fishTarget);
        }

        protected virtual void Eat(Fish targetFish)
        {
            if (targetFish == default)
                return;

            if (!CanEat())
                return;

            targetFish.Eaten();
        }

        protected virtual void Eaten()
        {
            Destroy(gameObject);
        }

        private bool CanEat()
        {
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (mouthSize == default)
                return;

            Gizmos.DrawWireSphere(mouthPos.position, mouthSize);
        }
    }
}