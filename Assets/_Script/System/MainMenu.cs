using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void OnPlay()
    {
        gameManager.ChangeState(GameState.GamePlay);
    }

    public void OnQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
