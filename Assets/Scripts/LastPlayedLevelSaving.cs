using UnityEngine;
using UnityEngine.SceneManagement;

public class LastPlayedLevelSaving : MonoBehaviour
{
    private const string LastSceneIndexKey = "Last Scene Index";
    private const string LastSceneNumberKey = "Last Scene Number";
    private const int DefaultSceneNumber = 1;
    
    private int _sceneNumber;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(LastSceneNumberKey))
        {
            _sceneNumber = DefaultSceneNumber;
            PlayerPrefs.SetInt(LastSceneNumberKey, _sceneNumber);
        }
        else
            _sceneNumber = PlayerPrefs.GetInt(LastSceneNumberKey);
        
        PlayerPrefs.SetInt(LastSceneIndexKey, SceneManager.GetActiveScene().buildIndex);
    }

    public int GetSceneNumber() => _sceneNumber;

}
