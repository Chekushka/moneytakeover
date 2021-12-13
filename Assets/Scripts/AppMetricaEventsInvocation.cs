using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppMetricaEventsInvocation : MonoBehaviour
{
    private LastPlayedLevelSaving _levelSaving;
    
    private const string StartLevelEventName = "level_start";
    private const string EndLevelEventName = "level_finish";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
        BuildingsCounting.OnPlayerWin += OnLevelWin;
        BuildingsCounting.OnPlayerFail += OnLevelFail;
    }

    private void OnDisable()
    { 
        SceneManager.sceneLoaded -= OnLevelLoaded;
        BuildingsCounting.OnPlayerWin -= OnLevelWin;
        BuildingsCounting.OnPlayerFail -= OnLevelFail;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode) => StartCoroutine(WaitForLevelLoading());

    private void OnLevelWin()
    {
        OnLevelFinished(true);
        BuildingsCounting.OnPlayerWin -= OnLevelWin;
    }

    private void OnLevelFail()
    {
        OnLevelFinished(false);
        BuildingsCounting.OnPlayerFail -= OnLevelFail;
    }

    private void OnLevelFinished(bool isWin)
    {
        var parameters = new Dictionary<string, object>();
        
        parameters.Add("level_number", _levelSaving.GetSceneIndex());
        parameters.Add("level_name", "Level " + _levelSaving.GetSceneNumber());
        parameters.Add("level_count", _levelSaving.GetSceneNumber());
        parameters.Add("level_loop", SceneManager.sceneCountInBuildSettings - 1);
        parameters.Add("level_random", false);
        parameters.Add("level_type", "normal");
        parameters.Add("game_mode", "classic");
        parameters.Add("result", isWin ? "win" : "lose");
        parameters.Add("time", Time.unscaledTime);

        AppMetrica.Instance.ReportEvent(EndLevelEventName, parameters);
        AppMetrica.Instance.SendEventsBuffer();
    }

    private void OnApplicationQuit()
    {
        var parameters = new Dictionary<string, object>();
        
        parameters.Add("level_number", _levelSaving.GetSceneIndex());
        parameters.Add("level_name", "Level " + _levelSaving.GetSceneNumber());
        parameters.Add("level_count", _levelSaving.GetSceneNumber());
        parameters.Add("level_loop", SceneManager.sceneCountInBuildSettings - 1);
        parameters.Add("level_random", false);
        parameters.Add("level_type", "normal");
        parameters.Add("game_mode", "classic");
        parameters.Add("result", "leave");
        parameters.Add("time", Time.unscaledTime);

        AppMetrica.Instance.ReportEvent(EndLevelEventName, parameters);
        AppMetrica.Instance.SendEventsBuffer();
    }

    private IEnumerator WaitForLevelLoading()
    {
        yield return new WaitForSeconds(1);
        _levelSaving = FindObjectOfType<LastPlayedLevelSaving>();
        
        var parameters = new Dictionary<string, object>();
        
        parameters.Add("level_number", _levelSaving.GetSceneIndex());
        parameters.Add("level_name", "Level " + _levelSaving.GetSceneNumber());
        parameters.Add("level_count", _levelSaving.GetSceneNumber());
        parameters.Add("level_loop", SceneManager.sceneCountInBuildSettings - 1);
        parameters.Add("level_random", false);
        parameters.Add("level_type", "normal");
        parameters.Add("game_mode", "classic");
        
        AppMetrica.Instance.ReportEvent(StartLevelEventName, parameters);
        AppMetrica.Instance.SendEventsBuffer();
    }
}
