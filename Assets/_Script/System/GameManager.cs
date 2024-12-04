using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] CinemachineVirtualCamera _mainMenuCam;

    [SerializeField] Canvas _hudCanvas;
    [SerializeField] Canvas _mainMenuCanvas;


    [SerializeField] GameState _currGameState = GameState.MainMenu;
    public GameState GetCurrGameState { get => _currGameState; }

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
}

public enum GameState
{
    MainMenu,
    GamePlay,
    GamePause,
}
