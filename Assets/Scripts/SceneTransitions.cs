using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private const string LastSceneNumberKey = "Last Scene Number";
    private const string LastSceneIndexKey = "Last Scene Index";
    private int _sceneNumber;

    public void RestartScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadNextScene()
    {
        _sceneNumber = PlayerPrefs.GetInt(LastSceneNumberKey);
        _sceneNumber++;
        PlayerPrefs.SetInt(LastSceneNumberKey, _sceneNumber);
        var sceneIndex = PlayerPrefs.GetInt(LastSceneIndexKey);
        
        if (sceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            sceneIndex = 2;
        else
            sceneIndex++;  
        
        SceneManager.LoadScene(sceneIndex);
    }
}
