using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnvironment : MonoBehaviour
{
    [SerializeField] List<Fish> _fishPrefabs = new();

    [Header("Settings")]
    [SerializeField] int _fishStartAmount;
    [SerializeField] int _fishStartOffScreenAmount;
    [SerializeField] Transform parentGroup;

    [Header("SpawnAreas")]
    [SerializeField] RectTransform _startSpawnArea;
    [SerializeField] RectTransform[] _offScreenSpawnArea;

    [Header("Scaler")]
    [SerializeField, Range(0.5f, 1f)] float _sizeMinScale = 1;
    [SerializeField, Range(0.5f, 1f)] float _sizeMaxScale = 1;

    [SerializeField, Range(0.5f, 1f)] float _speedMinScale = 1;
    [SerializeField, Range(0.5f, 1f)] float _speedMaxScale = 1;

    void Start()
    {
        SpawnFish(_startSpawnArea);
        SpawnFish(_fishStartOffScreenAmount);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Fish fish;

        if (fish = col.transform.GetComponent<Fish>())
        {
            fish.Flip();
        }

    }

    /// <summary>
    /// Spawn fish in start Area. Should only use at Start
    /// </summary>
    /// <param name="spawnArea"></param>
    void SpawnFish(RectTransform spawnArea)
    {
        Vector2 spawnAreaMin = new(spawnArea.rect.min.x + spawnArea.position.x, spawnArea.rect.min.y + spawnArea.position.y);
        Vector2 spawnAreaMax = new(spawnArea.rect.max.x + spawnArea.position.x, spawnArea.rect.max.y + spawnArea.position.y);

        for (int i = 0; i < _fishStartAmount; i++)
        {
            CreateFish(spawnAreaMin, spawnAreaMax, index: i);
        }

    }

    /// <summary>
    /// Spawn fish in offScreen
    /// </summary>
    /// <param name="amount"></param>
    public void SpawnFish(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //Random SpawnArea
            RectTransform spawnArea = _offScreenSpawnArea[Random.Range(0, _offScreenSpawnArea.Length)];
            Vector2 spawnAreaMin = new(spawnArea.rect.min.x + spawnArea.position.x, spawnArea.rect.min.y + spawnArea.position.y);
            Vector2 spawnAreaMax = new(spawnArea.rect.max.x + spawnArea.position.x, spawnArea.rect.max.y + spawnArea.position.y);

            CreateFish(spawnAreaMin, spawnAreaMax, index: i);
        }

    }

    void CreateFish(Vector2 spawnAreaMin, Vector2 spawnAreaMax, int index = 1)
    {
        Fish fish = Instantiate(_fishPrefabs[Random.Range(0, _fishPrefabs.Count)]);
        fish.transform.parent = parentGroup;

        //Set SpawnPos
        Vector2 spawnPos;
        spawnPos.x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        spawnPos.y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        fish.transform.position = spawnPos;

        //Flip Odd
        if (index % 2 == 0)
            fish.Flip();

        //Scaling
        fish.transform.localScale *= Random.Range(_sizeMinScale, _sizeMaxScale);
        fish.Speed *= Random.Range(_speedMinScale, _speedMaxScale);
    }
}
