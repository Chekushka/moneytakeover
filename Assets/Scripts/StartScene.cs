using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 1;

    private void Start() => SceneManager.LoadScene(sceneIndex);
}
