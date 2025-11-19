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

        private List<AiFish> aiFishList = new List<AiFish>();
        private JobHandle jobHandle;

        public const float MAX_SEARCH_DISTANCE = 30f;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (aiFishList.Count == 0)
                return;

            var inputData = new NativeArray<FishJobInput>(aiFishList.Count, Allocator.TempJob);
            var outputResults = new NativeArray<FishJobOutput>(aiFishList.Count, Allocator.TempJob);

            // Get data
            for (int i = 0; i < aiFishList.Count; i++)
            {
                var fish = aiFishList[i];
                var position = fish.transform.position;

                inputData[i] = new FishJobInput
                {
                    Index = i,
                    Position = new float2(position.x, position.y),
                    Size = fish.GetSize(),
                    CurrentStateInt = (int)fish.CurrentState
                };
            }

            // Job Scheduling
            var aiJob = new FishAIJob
            {
                AiFishesInput = inputData,
                OutputResults = outputResults,
                MaxSearchDistance = MAX_SEARCH_DISTANCE * MAX_SEARCH_DISTANCE
            };

            jobHandle = aiJob.Schedule(aiFishList.Count, jobHandle);

            // Wait for job
            jobHandle.Complete();

            // Apply
            for (int i = 0; i < aiFishList.Count; i++)
            {
                var fish = aiFishList[i];
                var output = outputResults[i];

                fish.TargetPosition = output.TargetPosition;
                fish.CurrentState = (State)output.StateInt;

                fish.UpdateMovement();
            }

            inputData.Dispose();
            outputResults.Dispose();
        }

        public void FetchAllFish()
        {
            var aiFishes = FindObjectsByType<AiFish>(sortMode: FindObjectsSortMode.None);
            aiFishList.AddRange(aiFishes);

            Debug.Log($"[AiFishManager] Fetched {aiFishes.Length} AI Fishes.");
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
        public NativeArray<FishJobInput> AiFishesInput;

        [ReadOnly]
        public float MaxSearchDistance;

        // Output
        [WriteOnly]
        public NativeArray<FishJobOutput> OutputResults;

        public void Execute(int index)
        {
            // Self
            var ownInput = AiFishesInput[index];
            var ownSize = ownInput.Size;
            var ownPosition = ownInput.Position;
            var closestTarget = MaxSearchDistance;

            // Target
            var newTargetPos = ownInput.Position;
            var newState = ownInput.CurrentStateInt;

            var isFoundTarget = false;

            // Find target
            for (int i = 0; i < AiFishesInput.Length; i++)
            {
                // Skip self
                if (i == index)
                    continue;

                var otherFish = AiFishesInput[i];
                var distance = math.lengthsq(otherFish.Position - ownPosition);

                // Found Prey
                if (otherFish.Size < ownSize * 0.9f)
                {
                    // Hunt
                    closestTarget = distance;
                    newTargetPos = otherFish.Position;
                    newState = (int)State.Hunting;
                    isFoundTarget = true;
                }

                // Found Predator
                else if (otherFish.Size > ownSize * 1.1f)
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
                        var randomSeed = (uint)(ownInput.Index * 1000f + ownPosition.x * 100 + ownPosition.y * 100 + 100);
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
    }
}