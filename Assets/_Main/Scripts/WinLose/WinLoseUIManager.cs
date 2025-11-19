using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseUIManager : MonoBehaviour
{
    [SerializeField]
    private string mainSceneName = "Default";
    [SerializeField]
    private string winSceneName = "WinScene";
    [SerializeField]
    private string loseSceneName = "LoseScene";

    public void MainButton()
    {
        Debug.Log("Main");
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(mainSceneName);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    [Button]
    public void WinButton()
    {
        Debug.Log("Win");
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(winSceneName);
    }

    [Button]
    public void LoseButton()
    {
        Debug.Log("Lose");
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(loseSceneName);
    }

}
