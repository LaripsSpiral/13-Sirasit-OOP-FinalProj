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

        [Header("KnockBack Settings")]
        [SerializeField]
        private float knockBackRange = 1.0f;
        [SerializeField]
        private float knockBackForce = 1.0f;

        [Header("Dash Settings")]
        [SerializeField]
        private float dashForce = 2f;
        [SerializeField]
        private float dashCooldown = 1f;
        private float lastDashTime = -100f;

        private ScoreSystem scoreSystem => ScoreSystem.Instance;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Move(moveDir);
        }

        public void Dash()
        {

            Vector2 dir = moveDir.normalized;
            if (dir == Vector2.zero)
            {
                if (Rb2d.linearVelocity.sqrMagnitude > 0.1f)
                    dir = Rb2d.linearVelocity.normalized;
                else
                    dir = transform.right;
            }

            Rb2d.AddForce(dir * dashForce, ForceMode2D.Impulse);
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
            transform.localScale += Vector3.one * (targetFish.GetSize() / 200) + (Vector3.one * comboCount/100000);
            Debug.Log($"[Player Size] update : {GetSize()}");
            OnAte?.Invoke(targetFish.GetSize());

            CreateKnockbackWave(radius: knockBackRange, baseForce: knockBackForce);

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

            CreateKnockbackWave(radius: knockBackRange, baseForce: knockBackForce);

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

        private void CreateKnockbackWave(float radius, float baseForce)
        {
            // Collect all colliders in radius
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
            if (hits == null || hits.Length == 0)
                return;

            Vector2 center = transform.position;
            const float epsilon = 0.0001f;

            foreach (var col in hits)
            {
                if (col.attachedRigidbody == null)
                    continue;

                var rb = col.attachedRigidbody;

                // Don't apply to self
                if (rb.gameObject == gameObject)
                    continue;

                Vector2 otherPos = rb.position;
                Vector2 dir = otherPos - center;
                float dist = dir.magnitude;

                if (dist < epsilon)
                {
                    // If overlapping exactly, push in a random direction to avoid NaN
                    dir = UnityEngine.Random.insideUnitCircle.normalized;
                    dist = epsilon;
                }

                // Attenuate force by distance (closer => stronger)
                float attenuation = Mathf.Clamp01(1f - (dist / radius));
                float forceMagnitude = baseForce * attenuation;

                Vector2 force = dir.normalized * forceMagnitude;

                // Add a slight upward bias (relative to world up) for visual variety
                force += Vector2.up * (attenuation * baseForce);
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}