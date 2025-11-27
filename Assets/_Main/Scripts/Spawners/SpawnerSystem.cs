using Main.Character;
using Main.Character.AI;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Main
{
    public class SpawnerSystem : MonoBehaviour
    {
        public static SpawnerSystem Instance;

        [SerializeField] private Transform parentGroup;

        [Header("SpawnAreas")]
        [SerializeField] private RectTransform _startSpawnArea;
        [SerializeField] private RectTransform[] _offScreenSpawnArea;

        [Header("Test")]
        [SerializeField] private Fish[] testFishPrefabs;
        [SerializeField] private float testScale = 1f;
        [SerializeField] private int testAmount = 10;

        // Dictionary to hold a specific pool for each specific Fish Prefab
        private Dictionary<Fish, ObjectPool<Fish>> _fishPools = new();

        // Dictionary to track which prefab a specific active fish instance came from (needed for returning)
        private Dictionary<Fish, Fish> _activeFishSourceMap = new();

        private void Awake()
        {
            Instance = this;
        }

        [Button]
        private void TestSpawnFish() => SpawnFishInArea(testFishPrefabs, testScale, testAmount);

        /// <summary>
        /// Retrieves the specific pool for a prefab, or creates one if it doesn't exist.
        /// </summary>
        private ObjectPool<Fish> GetPoolForPrefab(Fish prefab)
        {
            if (_fishPools.TryGetValue(prefab, out var pool)) return pool;

            // Create a new pool for this specific prefab type
            var newPool = new ObjectPool<Fish>(
                createFunc: () => {
                    // Instantiate under parentGroup immediately to avoid re-parenting cost later
                    return Instantiate(prefab, parentGroup);
                },
                actionOnGet: (fish) => fish.gameObject.SetActive(true),
                actionOnRelease: (fish) => fish.gameObject.SetActive(false),
                actionOnDestroy: (fish) => Destroy(fish.gameObject),
                defaultCapacity: 10,
                maxSize: 100
            );

            _fishPools.Add(prefab, newPool);
            return newPool;
        }

        public void SpawnFishInArea(Fish[] spawningFish, float scale, int amount)
        {
            // Pre-calculate world space rect only once if possible, 
            // but since startArea is one object, we do it here.
            var spawnArea = _startSpawnArea;
            Vector2 spawnAreaMin = GetWorldRectMin(spawnArea);
            Vector2 spawnAreaMax = GetWorldRectMax(spawnArea);

            for (int i = 0; i < amount; i++)
            {
                var fishPrefab = spawningFish[Random.Range(0, spawningFish.Length)];
                CreateFish(fishPrefab, scale, spawnAreaMin, spawnAreaMax);
            }

            // OPTIMIZATION: Only fetch AI once after the entire batch is spawned
            AiFishManager.Instance.FetchAllFish();
        }

        public void SpawnFish(Fish[] spawningFish, float scale, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var fishPrefab = spawningFish[Random.Range(0, spawningFish.Length)];

                // Random SpawnArea per fish
                RectTransform spawnArea = _offScreenSpawnArea[Random.Range(0, _offScreenSpawnArea.Length)];

                CreateFish(fishPrefab, scale, GetWorldRectMin(spawnArea), GetWorldRectMax(spawnArea));
            }

            // OPTIMIZATION: Only fetch AI once after the entire batch is spawned
            AiFishManager.Instance.FetchAllFish();
        }

        private void CreateFish(Fish fishPrefab, float scale, Vector2 spawnAreaMin, Vector2 spawnAreaMax)
        {
            // Get from the specific pool belonging to this prefab
            Fish fishInstance = GetPoolForPrefab(fishPrefab).Get();

            // Map the instance to the prefab so we know where to return it later
            _activeFishSourceMap[fishInstance] = fishPrefab;

            // Set SpawnPos
            Vector2 spawnPos;
            spawnPos.x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            spawnPos.y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            fishInstance.transform.position = spawnPos;
            fishInstance.transform.localScale = Vector3.one * scale;

            // Assuming SetSpeed initializes the fish logic
            fishInstance.SetSpeed(fishInstance.Speed);
        }

        /// <summary>
        /// CALL THIS instead of Destroy(fish.gameObject)
        /// </summary>
        public void ReturnFishToPool(Fish fishInstance)
        {
            if (_activeFishSourceMap.TryGetValue(fishInstance, out Fish sourcePrefab))
            {
                // Return to the correct pool
                if (_fishPools.TryGetValue(sourcePrefab, out var pool))
                {
                    pool.Release(fishInstance);
                    _activeFishSourceMap.Remove(fishInstance);
                    return;
                }
            }

            // Fallback if not found in pool map (e.g. scene objects)
            Destroy(fishInstance.gameObject);
        }

        // Helper to keep code clean
        private Vector2 GetWorldRectMin(RectTransform rt) =>
            new Vector2(rt.rect.min.x, rt.rect.min.y) + (Vector2)rt.position;

        private Vector2 GetWorldRectMax(RectTransform rt) =>
            new Vector2(rt.rect.max.x, rt.rect.max.y) + (Vector2)rt.position;
    }
}