using Unity.Mathematics;
using UnityEngine;

namespace Main.Character.AI
{
    public class AiFish : Fish
    {
        public State CurrentState { get; set; }
        public float2 TargetPosition { get; set; }

        // Movement is handled on the main thread, consuming the Job's calculated target
        public void UpdateMovement()
        {
            float2 currentPos = new float2(transform.position.x, transform.position.y);
            float2 direction = TargetPosition - currentPos;

            Move(direction, ForceMode2D.Force, multiplier: 0.01f);
        }

        private void OnDrawGizmos()
        {
            // Draw fish state
            switch (CurrentState)
            {
                case State.Idle:
                    Gizmos.color = Color.blue;
                    break;

                case State.Hunting:
                    Gizmos.color = Color.yellow;
                    break;

                case State.Fleeing:
                    Gizmos.color = Color.green;
                    break;

                default:
                    Gizmos.color = Color.white;
                    break;
            }
            Gizmos.DrawWireSphere(transform.position, GetSize());


            // Draw velocity
            Gizmos.color = Color.orange;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(Rb2d.linearVelocity.x, Rb2d.linearVelocity.y, 0));
        }

        private void OnDrawGizmosSelected()
        {
            // Draw target position
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector3(TargetPosition.x, TargetPosition.y, 0));
            Gizmos.DrawWireSphere(new Vector3(TargetPosition.x, TargetPosition.y, 0), 0.05f);
        }
    }
}