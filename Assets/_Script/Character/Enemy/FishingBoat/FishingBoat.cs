using UnityEngine;

public class FishingBoat : Enemy
{
    [SerializeField] FishingBait _fishingBaitPrefab;

    public Transform BaitSpawnPoint;

    [SerializeField] float _minHookRange , _maxHookRange;

    FishingBait _fishBait;

    private void Start()
    {
        Behavior();
    }

    protected override void Behavior()
    {
        SpawnHook();
    }

    void SpawnHook()
    {
        if (_fishBait)
            _fishBait.HookUp();

        _fishBait = Instantiate(_fishingBaitPrefab, BaitSpawnPoint);
        _fishBait.Init(ownerBoat: this, hookRange: Random.Range(_minHookRange, _maxHookRange));

        Invoke(nameof(SpawnHook), Random.Range(5, 15));
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(BaitSpawnPoint.position + Vector3.down * _minHookRange, BaitSpawnPoint.position + Vector3.down * _maxHookRange); 
    }
}
