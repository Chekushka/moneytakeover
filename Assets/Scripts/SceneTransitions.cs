using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public void RestartScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadNextScene() => SceneManager.LoadScene(FindObjectOfType<LastPlayedLevelSaving>().GetSceneIndex());
}
