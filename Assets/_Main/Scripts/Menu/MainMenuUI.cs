using UnityEngine;
using UnityEngine.UI;

namespace Main.Menu
{
    public class MainMenuUI : BaseUI
    {
        private GameManager gameManager => GameManager.Instance;

        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button quitButton;

        private void Start()
        {
            playButton.onClick.AddListener(HandlePlayButtonClicked);
            quitButton.onClick.AddListener(HandleQuitButtonClicked);
        }

        private void HandlePlayButtonClicked()
        {
            Debug.Log("[MainMenuUI] Play button clicked");
            gameManager.StartGame();
        }

        private void HandleQuitButtonClicked()
        {
            Debug.Log("[MainMenuUI] Quit button clicked");
            Application.Quit();
        }
    }
}