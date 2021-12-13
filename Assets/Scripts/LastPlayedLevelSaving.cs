using Buildings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastPlayedLevelSaving : MonoBehaviour
{
    private const string LastSceneIndexKey = "LastScene_Index";
    private const string LastSceneNumberKey = "LastScene_Number";
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

    private void OnEnable() => BuildingsCounting.OnPlayerWin += OnLevelCompletedSave;

    private void OnDisable() => BuildingsCounting.OnPlayerWin -= OnLevelCompletedSave;

    public int GetSceneNumber() => _sceneNumber;
    public int GetSceneIndex() => PlayerPrefs.GetInt(LastSceneIndexKey);

    private void OnLevelCompletedSave()
    {
        _sceneNumber = PlayerPrefs.GetInt(LastSceneNumberKey);
        _sceneNumber++;
        PlayerPrefs.SetInt(LastSceneNumberKey, _sceneNumber);
        
        var sceneIndex = PlayerPrefs.GetInt(LastSceneIndexKey);
        
        if (sceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            sceneIndex = 2;
        else
            sceneIndex++;
        
        PlayerPrefs.SetInt(LastSceneIndexKey, sceneIndex);
        BuildingsCounting.OnPlayerWin -= OnLevelCompletedSave;
    }
}
