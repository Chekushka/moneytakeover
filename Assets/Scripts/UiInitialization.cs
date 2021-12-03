using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiInitialization : MonoBehaviour
{
    private Canvas _canvas;
    private const string UiCameraTag = "UiCamera"; 

    private void Awake() => _canvas = GetComponent<Canvas>();

    private void OnEnable() => SceneManager.sceneLoaded += OnLevelFinishedLoading;
    private void OnDisable() =>  SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        var cameras = FindObjectsOfType<Camera>().ToList();
        var uiCamera = cameras.Find(cam => cam.CompareTag(UiCameraTag));
        _canvas.worldCamera = uiCamera;
    }
}
