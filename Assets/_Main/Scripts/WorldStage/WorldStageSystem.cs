using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Main.WorldStage
{
    public class WorldStageSystem : MonoBehaviour
    {
        public static WorldStageSystem Instance;

        [SerializeField, ReadOnly]
        private Queue<WorldStageData> worldStageQueue = new();

        [SerializeField]
        private WorldStageData[] worldStageData;

        public void Awake()
        {
            Instance = this;
        }

        public void Init() 
        {
            worldStageQueue.Clear();

            foreach (var data in worldStageData)
            {
                worldStageQueue.Enqueue(data);
            }
        }
        public WorldStageData GetCurrentStage()
        {
            if (!CheckRemainingStage())
                return default;

            return worldStageQueue.Peek();
        }

        public WorldStageData NextStage()
        {
            if (!CheckRemainingStage())
                return default;

            worldStageQueue.Dequeue();
            return worldStageQueue.Peek();
        }

        private bool CheckRemainingStage()
        {
            if (worldStageQueue.Count == 0)
            {
                Debug.Log($"[WorldStageSystem] Out of stage");
                return false;
            }

            return true;
        }
    }
}