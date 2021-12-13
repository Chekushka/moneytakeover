using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] private int defaultSceneIndex = 1;
    [SerializeField] private bool isDebug;
    [SerializeField] private int debugIndex;

    private const string SceneSaveKey = "LastScene_Index";
    private void Start() => SceneManager.LoadScene(GetSceneToLoadIndex());
    
    private int GetSceneToLoadIndex()
    {
        int index;

        if (isDebug)
        {
            index = debugIndex;
            PlayerPrefs.DeleteAll();
        }
        else
            index = PlayerPrefs.HasKey(SceneSaveKey) ? PlayerPrefs.GetInt(SceneSaveKey) : defaultSceneIndex;

        return index;
    }
}
