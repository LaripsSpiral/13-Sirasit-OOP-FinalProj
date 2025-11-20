using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Main.Character.AI
{
    public class AiFishManager : MonoBehaviour
    {
        public static AiFishManager Instance;

        [SerializeField]
        private PlayerCharacter playerCharacter;

        private List<Fish> fishList = new List<Fish>();
        private JobHandle jobHandle;

        public const float VISION_RANGE = 5f;
        public const float VISION_ANGLE = 120f;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (fishList.Count == 0)
                return;

            var fishesCount = fishList.Count;
            var inputData = new NativeArray<FishJobInput>(fishesCount, Allocator.TempJob);
            var outputResults = new NativeArray<FishJobOutput>(fishesCount, Allocator.TempJob);

            // Get data
            for (int i = 0; i < fishesCount; i++)
            {
                // Fish Data
                var fish = fishList[i];
                var transform = fish.transform;
                var state = (int)State.Idle;

                // Get AI State if possible
                if (fish.TryGetComponent<AiFish>(out var aiFish))
                {
                    state = (int)aiFish.CurrentState;
                }

                // Fill Input Data
                inputData[i] = new FishJobInput
                {
                    Index = i,
                    Position = new float2(transform.position.x, transform.position.y),
                    ForwardDirection = new float2(transform.right.x, transform.right.y),
                    Size = fish.GetSize(),
                    CurrentStateInt = state
                };
            }

            // Job Scheduling
            var aiJob = new FishAIJob
            {
                FishesInput = inputData,
                OutputResults = outputResults,
                MaxSearchDistance = VISION_RANGE * VISION_RANGE,
                MaxVisionAngle = math.cos(math.radians(VISION_ANGLE) / 2f),
            };

            jobHandle = aiJob.Schedule(fishList.Count, jobHandle);

            // Wait for job
            jobHandle.Complete();

            // Apply
            for (int i = 0; i < fishList.Count; i++)
            {
                var fish = fishList[i];

                if (!fish.TryGetComponent<AiFish>(out var aiFish))
                    continue;

                var output = outputResults[i];
                aiFish.TargetPosition = output.TargetPosition;
                aiFish.CurrentState = (State)output.StateInt;

                aiFish.UpdateMovement();
            }

            inputData.Dispose();
            outputResults.Dispose();
        }

        public void FetchAllFish()
        {
            // Add Player
            var playerCharacter = FindAnyObjectByType<PlayerCharacter>();
            fishList.Add(playerCharacter);
            playerCharacter.OnDeath += () => fishList.Remove(playerCharacter);

            // Add AI Fishes
            var aiFishes = FindObjectsByType<AiFish>(sortMode: FindObjectsSortMode.None);
            foreach (var aiFish in aiFishes)
            {
                fishList.Add(aiFish);
                aiFish.OnDeath += () => fishList.Remove(aiFish);
            }

            Debug.Log($"[AiFishManager] Fetched {aiFishes.Length} Fishes.");
        }
    }

    public enum State
    {
        Idle = 0,
        Hunting = 1,
        Fleeing = 2,
    }

    public struct FishJobInput
    {
        public int Index;
        public float2 Position;
        public float2 ForwardDirection;
        public float Size;
        public int CurrentStateInt;
    }

    public struct FishJobOutput
    {
        public float2 TargetPosition;
        public int StateInt; // For Job
    }

    [BurstCompile]
    public struct FishAIJob : IJobFor
    {
        // Input
        [ReadOnly]
        public NativeArray<FishJobInput> FishesInput;

        [ReadOnly]
        public float MaxSearchDistance;

        [ReadOnly]
        public float MaxVisionAngle;

        // Output
        [WriteOnly]
        public NativeArray<FishJobOutput> OutputResults;

        public void Execute(int index)
        {
            // Self
            var ownInput = FishesInput[index];
            var ownSize = ownInput.Size;
            var ownPosition = ownInput.Position;
            var closestTarget = MaxSearchDistance;

            // Target
            var newTargetPos = ownInput.Position;
            var newState = ownInput.CurrentStateInt;

            var isFoundTarget = false;

            // Find target
            for (int i = 0; i < FishesInput.Length; i++)
            {
                // Skip self
                if (i == index)
                    continue;

                var otherFish = FishesInput[i];
                var distance = math.lengthsq(otherFish.Position - ownPosition);

                // Out of Vision Range
                if (distance > MaxSearchDistance)
                    continue;

                // Out of Vision Cone
                if (!IsInVisionCone(ownPosition, ownInput.ForwardDirection, otherFish.Position, MaxVisionAngle))
                    continue;

                // Found Prey
                if (otherFish.Size < ownSize)
                {
                    // Hunt
                    closestTarget = distance;
                    newTargetPos = otherFish.Position;
                    newState = (int)State.Hunting;
                    isFoundTarget = true;
                }

                // Found Predator
                else if (otherFish.Size > ownSize)
                {
                    // Fleeing
                    if (distance < closestTarget * 1.5f || newState != (int)State.Fleeing)
                    {
                        var fleeDir = math.normalize(ownPosition - otherFish.Position);
                        newTargetPos = ownPosition + fleeDir * 20;
                        newState = (int)State.Fleeing;
                        closestTarget = distance;
                        isFoundTarget = true;
                    }
                }
            }

            // No target
            if (!isFoundTarget && newState != (int)State.Fleeing)
            {
                switch (newState)
                {
                    case (int)State.Hunting:
                        newState = (int)State.Idle;
                        break;

                    case (int)State.Idle:

                        // Random Wandering Position
                        var randomSeed = (uint)(ownInput.Index * 1000f + ownPosition.x * 100 + ownPosition.y * 100) + 1000;
                        Unity.Mathematics.Random random = new Unity.Mathematics.Random(randomSeed);

                        newTargetPos.x = random.NextFloat(-50f, 50f);
                        newTargetPos.y = random.NextFloat(-50f, 50f);
                        newState = (int)State.Idle;
                        break;
                }
            }

            // Write output
            OutputResults[index] = new FishJobOutput
            {
                TargetPosition = newTargetPos,
                StateInt = newState,
            };
        }

        private static bool IsInVisionCone(float2 ownPos, float2 ownForward, float2 targetPos, float cosAngleThreshold)
        {
            float2 directionToTarget = math.normalize(targetPos - ownPos);
            float dotProduct = math.dot(ownForward, directionToTarget);

            // If the dot product is greater than the cosine of the half-angle, the target is in the cone.
            return dotProduct >= cosAngleThreshold;
        }
    }
}