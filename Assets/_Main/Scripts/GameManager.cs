using Main.Character.AI;
using Main.Menu;
using Main.Player;
using Main.Times;
using Main.WorldStage;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Ref/Camera")]
        [SerializeField]
        private CinemachineCamera mainMenuCamera;

        [SerializeField]
        private CinemachineCamera playerCamera;

        [Header("Ref/MainUI")]
        [SerializeField]
        private MainMenuUI mainMenuUI;

        [SerializeField]
        private PauseUI pauseUI;

        [Header("Player")]
        [SerializeField]
        private int maxPlayerHealth = 3;

        [SerializeField]
        private PlayerController player;

        [SerializeField]
        private PlayerHealthUI playerHealthUI;

        [Header("Progress")]
        [SerializeField, ReadOnly]
        private float currentProgress = 0;

        [SerializeField]
        private ProgressUI progressUI;

        [Header("WorldStage")]
        private WorldStageSystem worldStage => WorldStageSystem.Instance;
        private SpawnerSystem spawner => SpawnerSystem.Instance;

        [Header("FishSpawn")]
        [SerializeField]
        private int startSpawnAmount = 25;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            TimeScaleSystem.PauseTime();
            mainMenuCamera.gameObject.SetActive(true);
            playerCamera.gameObject.SetActive(false);
            mainMenuUI.ToggleShow(true);
        }

        public void StartGame()
        {
            Debug.Log("[GameManager] Setup Start");
            TimeScaleSystem.Resume();

            mainMenuCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);

            // Setup
            player.Character.Setup(health: maxPlayerHealth);
            player.Character.OnTakeDamage += () => playerHealthUI.UpdateHeartUI(player.Character.CurrentHealth);
            player.Character.OnAte += AddProgres;
            player.Character.OnDeath += EndGame;

            worldStage.Init();
            var spawningFishes = worldStage.GetCurrentStage().SpawningFishes;
            spawner.SpawnFish(spawningFishes, amount: startSpawnAmount);

            Debug.Log("[GameManager] Started Game");
            AiFishManager.Instance.FetchAllFish();
        }

        private void EndGame()
        {
            Debug.Log("[GameManager] Ended Game");
            TimeScaleSystem.PauseTime();

            pauseUI.ToggleShow(false);
        }

        private void AddProgres(float addValue)
        {
            currentProgress += addValue;
            progressUI.UpdateUI(currentProgress);

            // Win
            if (currentProgress > 100)
            {
                EndGame();
            }
        }
    }
}