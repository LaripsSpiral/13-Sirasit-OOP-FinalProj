using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Ref/Cam")]
    [SerializeField] CinemachineCamera _mainMenuCam;

    [Header("Ref/UI")]
    [SerializeField] Canvas _hudCanvas;
    [SerializeField] Canvas _mainMenuCanvas;

    [Header("Ref/UI/Pause")]
    [SerializeField] Canvas _pauseSceneCanvas;
    [SerializeField] GameObject _wonPanel, _lostPanel;


    [Header("State")]
    [SerializeField] GameState _currGameState = GameState.MainMenu;
    public GameState CurrGameState { get => _currGameState; }

    private void Start()
    {
        Debug.Log("Start");
        ChangeState(_currGameState);
    }

    public void ChangeState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                MainMenu();
                break;

            case GameState.GamePlay:
                Gameplay();
                break;

            case GameState.GamePause:
                break;
        }

        _currGameState = gameState;
        Debug.Log($"Change State to {_currGameState}");
    }

    void MainMenu()
    {
        _mainMenuCam.gameObject.SetActive(true);
        _mainMenuCanvas.gameObject.SetActive(true);

        _hudCanvas.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }
    
    void Gameplay()
    {
        _mainMenuCam.gameObject.SetActive(false);
        _mainMenuCanvas.gameObject.SetActive(false);

        _hudCanvas.gameObject.SetActive(true);

        Time.timeScale = 1f;
    }

    void GamePause()
    {
        _pauseSceneCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameEnd(bool isWon)
    {
        GamePause();

        _wonPanel.SetActive(isWon);
        _lostPanel.SetActive(!isWon);
    }
}

public enum GameState
{
    MainMenu,
    GamePlay,
    GamePause,
}
