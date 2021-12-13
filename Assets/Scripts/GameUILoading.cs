using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUILoading : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject failScreen;
    [SerializeField] private GameObject failText;
    [SerializeField] private GameObject restartButton;

    private void OnEnable() => SceneManager.sceneLoaded += OnLevelFinishedLoading;
    private void OnDisable() =>  SceneManager.sceneLoaded -= OnLevelFinishedLoading;

    public GameObject GetWinScreen() => winScreen;
    public GameObject GetFailScreen() => failScreen;
    
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        winScreen.SetActive(false);
        winText.transform.localScale = Vector3.zero;
        
        failScreen.SetActive(false);
        failText.transform.localScale = Vector3.zero;
        
        restartButton.SetActive(true);
    }
    
}