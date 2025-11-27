using Main.Character;
using Main.Character.AI;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main
{
    public class SpawnerSystem : MonoBehaviour
    {
        public static SpawnerSystem Instance;

        [SerializeField]
        private Transform parentGroup;

        [Header("SpawnAreas")]
        [SerializeField] private RectTransform _startSpawnArea;

        [SerializeField] private RectTransform[] _offScreenSpawnArea;

        [Header("Test")]
        [SerializeField]
        private Fish[] testFishPrefabs;

        [SerializeField]
        private float testScale;

        [SerializeField]
        private int testAmount;

        private void Awake()
        {
            Instance = this;
        }

        [Button]
        private void TestSpawnFish() => SpawnFishInArea(testFishPrefabs, testScale, testAmount);

        /// <summary>
        /// Spawn fish in start Area. Should only use at Start
        /// </summary>
        /// <param name="spawnArea"></param>
        public void SpawnFishInArea(Fish[] spawningFish, float scale, int amount)
        {
            var spawnArea = _startSpawnArea;
            Vector2 spawnAreaMin = new(spawnArea.rect.min.x + spawnArea.position.x, spawnArea.rect.min.y + spawnArea.position.y);
            Vector2 spawnAreaMax = new(spawnArea.rect.max.x + spawnArea.position.x, spawnArea.rect.max.y + spawnArea.position.y);

            for (int i = 0; i < amount; i++)
            {
                var fish = spawningFish[UnityEngine.Random.Range(0, spawningFish.Length)];
                CreateFish(fishPrefabs: fish, scale: scale, spawnAreaMin, spawnAreaMax);
            }
        }

        /// <summary>
        /// Spawn fish in offScreen
        /// </summary>
        /// <param name="amount"></param>
        public void SpawnFish(Fish[] spawningFish, float scale, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var fish = spawningFish[UnityEngine.Random.Range(0, spawningFish.Length)];

                //Random SpawnArea
                RectTransform spawnArea = _offScreenSpawnArea[Random.Range(0, _offScreenSpawnArea.Length)];

                Vector2 spawnAreaMin = new(
                    spawnArea.rect.min.x + spawnArea.position.x,
                    spawnArea.rect.min.y + spawnArea.position.y);

                Vector2 spawnAreaMax = new(
                    spawnArea.rect.max.x + spawnArea.position.x,
                    spawnArea.rect.max.y + spawnArea.position.y);

                CreateFish(fishPrefabs: fish, scale: scale, spawnAreaMin, spawnAreaMax);
            }
        }

        /// <summary>
        /// Create Fish with random Size and Speed by randomScale
        /// </summary>
        /// <param name="fishPrefabs"></param>
        /// <param name="spawnAreaMin"></param>
        /// <param name="spawnAreaMax"></param>
        private void CreateFish(Fish fishPrefabs, float scale, Vector2 spawnAreaMin, Vector2 spawnAreaMax)
        {
            Fish fish = Instantiate(fishPrefabs);
            fish.transform.parent = parentGroup;

            //Set SpawnPos
            Vector2 spawnPos;
            spawnPos.x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            spawnPos.y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            fish.transform.position = spawnPos;

            fish.transform.localScale = Vector3.one * scale;
            fish.SetSpeed(fish.Speed);

            AiFishManager.Instance.FetchAllFish();
        }
    }
}