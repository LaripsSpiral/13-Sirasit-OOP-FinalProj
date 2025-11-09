using Main.Menu;
using Main.Player;
using Main.Times;
using Main.WorldStage;
using Unity.Cinemachine;
using UnityEngine;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private WorldStageSystem worldStage => WorldStageSystem.Instance;
        private SpawnerSystem spawner => SpawnerSystem.Instance;

        [Header("Ref/Camera")]
        [SerializeField]
        private CinemachineCamera mainMenuCamera;

        [SerializeField]
        private CinemachineCamera playerCamera;

        [Header("Ref/UI")]
        [SerializeField]
        private MainMenuUI mainMenuUI;
        [SerializeField]
        private PauseUI pauseUI;

        [Header("Player")]
        [SerializeField]
        private int playerHealth = 3;

        [SerializeField]
        private PlayerController player;

        [Header("FishSpawn")]
        [SerializeField]
        private int startSpawnAmount = 25;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            TimeSystem.PauseTime();
            mainMenuCamera.gameObject.SetActive(true);
            playerCamera.gameObject.SetActive(false);
            mainMenuUI.ToggleShow(true);
        }

        public void StartGame()
        {
            Debug.Log("[GameManager] Setup Start");
            TimeSystem.Resume();

            mainMenuCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);

            // Setup
            player.Character.Setup(health: playerHealth);
            player.Character.OnDeath += EndGame;

            worldStage.Init();
            var spawningFishes = worldStage.GetCurrentStage().SpawningFishes;
            spawner.SpawnFish(spawningFishes, amount: startSpawnAmount);

            Debug.Log("[GameManager] Started Game");
        }

        private void EndGame()
        {
            Debug.Log("[GameManager] Ended Game");
            TimeSystem.PauseTime();

            pauseUI.ToggleShow(false);
        }
    }
}