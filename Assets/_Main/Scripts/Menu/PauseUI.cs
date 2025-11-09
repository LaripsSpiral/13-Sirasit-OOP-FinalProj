using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main.Menu
{
    public class PauseUI : BaseUI
    {
        [SerializeField]
        private Button mainMenuButton;

        [SerializeField]
        private Button quitButton;

        private void Start()
        {
            mainMenuButton.onClick.AddListener(HandleMainMenuButtonClicked);
            quitButton.onClick.AddListener(HandleQuitButtonClicked);
        }

        private void HandleMainMenuButtonClicked()
        {
            Debug.Log("[MainMenuUI] MainMenu button clicked");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void HandleQuitButtonClicked()
        {
            Debug.Log("[MainMenuUI] Quit button clicked");
            Application.Quit();
        }
    }
}