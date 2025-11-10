using Unity.Mathematics;

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

            Move(direction, UnityEngine.ForceMode2D.Force, 0.5f);
        }
    }
}