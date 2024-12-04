using UnityEngine;

public class FishingBoat : Enemy
{
    [SerializeField] FishingBait _fishingBaitPrefab;

    public Transform BaitSpawnPoint;
    [SerializeField] RectTransform _fishingArea;

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
        _fishBait = Instantiate(_fishingBaitPrefab, BaitSpawnPoint);
        _fishBait.Init(ownerBoat: this, areaRectT: _fishingArea);
    }
}
