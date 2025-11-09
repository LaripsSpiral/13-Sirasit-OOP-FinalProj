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
            //Get Rotate Angle
            Vector2 dir = Rb2d.linearVelocity;
            float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //Flip Rotate Angle
            rotAngle -= dir.x < 0 ? -180 : 0;

            transform.rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);

            //Flip Character
            if (dir.x != 0)
            {
                Vector3 flipScale = transform.localScale;
                flipScale.x = dir.x < 0 ? -1 : 1;

                transform.localScale = flipScale;
            }
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