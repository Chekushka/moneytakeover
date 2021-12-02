using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScreen : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject failScreen;

    private void OnEnable() => SceneManager.sceneLoaded += OnLevelFinishedLoading;
    private void OnDisable() =>  SceneManager.sceneLoaded -= OnLevelFinishedLoading;

    public GameObject GetWinScreen() => winScreen;
    public GameObject GetFailScreen() => failScreen;
    
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        winScreen.SetActive(false);
        failScreen.SetActive(false);
    }
}