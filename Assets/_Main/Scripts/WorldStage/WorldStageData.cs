using Main.Character;
using UnityEngine;

namespace Main.WorldStage
{
    [CreateAssetMenu(fileName = "NewWorldStageData", menuName = "Main/SO/WorldStageData")]
    public class WorldStageData : ScriptableObject
    {
        [SerializeField]
        private GameObject sky;

        [SerializeField]
        private GameObject decoration;

        [SerializeField]
        private Fish[] spawningFishes;

        public GameObject GetSky() => sky;
        public GameObject GetDecoration() => decoration;
        public Fish[] GetSpawningFishes() => spawningFishes;
    }
}