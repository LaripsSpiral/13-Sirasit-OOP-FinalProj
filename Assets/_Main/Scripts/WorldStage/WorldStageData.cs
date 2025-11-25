using Main.Character;
using UnityEngine;

namespace Main.WorldStage
{
    [CreateAssetMenu(fileName = "NewWorldStageData", menuName = "Main/SO/WorldStageData")]
    public class WorldStageData : ScriptableObject
    {
        [SerializeField]
        private Vector2 randomSizeRange;

        [SerializeField]
        private GameObject sky;

        [SerializeField]
        private GameObject decoration;

        [SerializeField]
        private Fish[] spawningFishes;

        public Vector2 GetRandomSizeRange() => randomSizeRange;
        public GameObject GetSky() => sky;
        public GameObject GetDecoration() => decoration;
        public Fish[] GetSpawningFishes() => spawningFishes;
    }
}