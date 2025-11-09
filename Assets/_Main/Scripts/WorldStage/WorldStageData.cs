using Main.Character;
using UnityEngine;

namespace Main.WorldStage
{
    [CreateAssetMenu(fileName = "NewWorldStageData", menuName = "Main/SO/WorldStageData")]
    public class WorldStageData : ScriptableObject
    {
        [SerializeField]
        public Fish[] SpawningFishes;
    }
}