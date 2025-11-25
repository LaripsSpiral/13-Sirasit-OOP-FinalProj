using Main.Character.AI;
using Main.Menu;
using Main.Player;
using Main.Score;
using Main.Times;
using Main.WorldStage;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private const float MAX_PROGRESS = 0.35f;

        [SerializeField]
        private ProgressUI progressUI;

        [Header("WorldStage")]
        private WorldStageSystem worldStage => WorldStageSystem.Instance;
        private SpawnerSystem spawner => SpawnerSystem.Instance;

        [Header("FishSpawn")]
        [SerializeField]
        private int startSpawnAmount = 25;

        [Header("Scene Names")]
        [SerializeField]
        private string winSceneName = "WinScene";
        [SerializeField]
        private string loseSceneName = "LoseScene";

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

            ScoreSystem.Instance.Reset();
        }

        public void StartGame()
        {
            Debug.Log("[GameManager] Setup Start");
            TimeScaleSystem.Resume();

            mainMenuCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);

            progressUI.Init(player.Character.GetSize(), MAX_PROGRESS);
            ScoreUI.Instance.gameObject.SetActive(true);

            // Setup
            player.Character.Setup(health: maxPlayerHealth);
            player.Character.OnTakeDamage += () => playerHealthUI.UpdateHeartUI(player.Character.CurrentHealth);
            player.Character.OnAte += _ => UpdateProgress(player.Character.GetSize());
            player.Character.OnDeath += EndGame;

            if (spawner != default)
            {
                worldStage.Init();
                var currentStage = worldStage.GetCurrentStage();
                var spawningFishes = currentStage.GetSpawningFishes();
                var scale = currentStage.GetRandomSizeRange();
                spawner.SpawnFishInArea(spawningFishes, scale: Random.Range(scale.x, scale.y), amount: startSpawnAmount / 2);
                spawner.SpawnFish(spawningFishes, scale: Random.Range(scale.x, scale.y), amount: startSpawnAmount/2);
            }

            Debug.Log("[GameManager] Started Game");
        }

        private void EndGame()
        {
            Debug.Log("[GameManager] Ended Game");
            LoseGame();
        }

        private void UpdateProgress(float currSize)
        {
            currentProgress = currSize;
            progressUI.UpdateUI(currentProgress);

            switch (currentProgress)
            {
                case < 0.125f:
                    if (worldStage.GetCurrentStage() != worldStage.WorldStageData[0])
                    {
                        worldStage.NextStage();
                    }
                    break;

                case < 0.2f:
                    if (worldStage.GetCurrentStage() != worldStage.WorldStageData[1])
                    {
                        worldStage.NextStage();
                    }
                    break;

                case < 0.25f:
                    if (worldStage.GetCurrentStage() != worldStage.WorldStageData[2])
                    {
                        worldStage.NextStage();
                    }
                    break;

                case < 0.3f:
                    if (worldStage.GetCurrentStage() != worldStage.WorldStageData[3])
                    {
                        worldStage.NextStage();
                    }
                    break;

                case >= MAX_PROGRESS:
                    WinGame();
                    break;
            }
        }
        private void WinGame()
        {
            Debug.Log("Win");
            TimeScaleSystem.Resume();
            SceneManager.LoadScene(winSceneName);
        }

        private void LoseGame()
        {
            Debug.Log("Lose");
            TimeScaleSystem.Resume();
            SceneManager.LoadScene(loseSceneName);
        }
    }
}