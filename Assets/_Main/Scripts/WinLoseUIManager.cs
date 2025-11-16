using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseUIManager : MonoBehaviour
{
    [SerializeField]
    private string mainSceneName = "Default";

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

}
