using NaughtyAttributes;
using UnityEngine;

namespace Main.Character
{
    public abstract class BaseCharacter : MonoBehaviour
    {
        [SerializeField]
        protected float speed = 10;
        public float Speed => speed;

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
            Rb2d.AddForce(speed * multiplier * Time.fixedDeltaTime * moveDir, forceMode2D);
        }

        private void RotateAlongVelocity()
        {
            Vector2 dir = Rb2d.linearVelocity;

            if (dir.sqrMagnitude < 0.01f)
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