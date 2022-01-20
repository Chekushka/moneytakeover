using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public delegate void PlayerRestart();
    public static event PlayerRestart OnPlayerRestart;
    public void RestartScene()
    {
        OnPlayerRestart?.Invoke();
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadNextScene() => SceneManager.LoadScene(FindObjectOfType<LastPlayedLevelSaving>().GetSceneIndex());
}
