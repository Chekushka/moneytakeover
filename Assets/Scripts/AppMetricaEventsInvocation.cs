using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppMetricaEventsInvocation : MonoBehaviour
{
    private LastPlayedLevelSaving _levelSaving;
    private IYandexAppMetrica _appMetrica;
    private int _currentPlaysCount;
    private bool _isLevelFinished;
    
    private const string StartLevelEventName = "level_start";
    private const string EndLevelEventName = "level_finish";

    private void OnEnable()
    {
        _appMetrica = AppMetrica.Instance;
        SceneManager.sceneLoaded += OnLevelLoaded;
        BuildingsCounting.OnPlayerWin += OnLevelWin;
        BuildingsCounting.OnPlayerFail += OnLevelFail;
        SceneTransitions.OnPlayerRestart += OnLevelRestarted;
    }

    private void OnDisable()
    { 
        SceneManager.sceneLoaded -= OnLevelLoaded;
        BuildingsCounting.OnPlayerWin -= OnLevelWin;
        BuildingsCounting.OnPlayerFail -= OnLevelFail;
        SceneTransitions.OnPlayerRestart -= OnLevelRestarted;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitForLevelLoading());
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    private void OnLevelWin()
    {
        BuildingsCounting.OnPlayerWin -= OnLevelWin;
        if (!_isLevelFinished)
        {
            OnLevelFinished(true);
            _isLevelFinished = true;
        }
    }

    private void OnLevelFail()
    {
        BuildingsCounting.OnPlayerFail -= OnLevelFail;
        if (!_isLevelFinished)
        {
            OnLevelFinished(false);
            _isLevelFinished = true;
        }
    }

    private void OnLevelFinished(bool isWin)
    {
        var parameters = new Dictionary<string, object>();
        
        parameters.Add("level_number", _levelSaving.GetSceneIndex());
        parameters.Add("level_name", "Level " + _levelSaving.GetSceneNumber());
        parameters.Add("level_count", _currentPlaysCount);
        parameters.Add("level_loop", _levelSaving.GetLoopCount());
        parameters.Add("level_random", false);
        parameters.Add("level_type", "normal");
        parameters.Add("game_mode", "classic");
        parameters.Add("result", isWin ? "win" : "lose");
        parameters.Add("time", Time.timeSinceLevelLoad);
        
        Debug.Log("Level finished:" + (isWin ? "win" : "lose"));
        parameters.ToList().ForEach(x => Debug.Log(x)); 
        
        _appMetrica.ReportEvent(EndLevelEventName, parameters);
        _appMetrica.SendEventsBuffer();
    }
    
    private void OnLevelRestarted()
    {
        var parameters = new Dictionary<string, object>();
        
        parameters.Add("level_number", _levelSaving.GetSceneIndex());
        parameters.Add("level_name", "Level " + _levelSaving.GetSceneNumber());
        parameters.Add("level_count", _currentPlaysCount);
        parameters.Add("level_loop", _levelSaving.GetLoopCount());
        parameters.Add("level_random", false);
        parameters.Add("level_type", "normal");
        parameters.Add("game_mode", "classic");
        parameters.Add("result", "restart");
        parameters.Add("time", Time.timeSinceLevelLoad);
        
        Debug.Log("Level Restarted");
        parameters.ToList().ForEach(x => Debug.Log(x)); 

        _appMetrica.ReportEvent(EndLevelEventName, parameters);
        _appMetrica.SendEventsBuffer();
    }

    private void OnApplicationQuit()
    {
        var parameters = new Dictionary<string, object>();
        
        parameters.Add("level_number", _levelSaving.GetSceneIndex());
        parameters.Add("level_name", "Level " + _levelSaving.GetSceneNumber());
        parameters.Add("level_count", _currentPlaysCount);
        parameters.Add("level_loop", _levelSaving.GetLoopCount());
        parameters.Add("level_random", false);
        parameters.Add("level_type", "normal");
        parameters.Add("game_mode", "classic");
        parameters.Add("result", "leave");
        parameters.Add("time", Time.timeSinceLevelLoad);

        _appMetrica.ReportEvent(EndLevelEventName, parameters);
        _appMetrica.SendEventsBuffer();
    }

    private IEnumerator WaitForLevelLoading()
    {
        yield return new WaitForSeconds(0.5f);

        _levelSaving = FindObjectOfType<LastPlayedLevelSaving>();
        _currentPlaysCount = _levelSaving.GetPlaysCount();
        
        var parameters = new Dictionary<string, object>();

        parameters.Add("level_number", _levelSaving.GetSceneIndex());
        parameters.Add("level_name", "Level " + _levelSaving.GetSceneNumber());
        parameters.Add("level_count", _currentPlaysCount);
        parameters.Add("level_loop", _levelSaving.GetLoopCount());
        parameters.Add("level_random", false);
        parameters.Add("level_type", "normal");
        parameters.Add("game_mode", "classic");
        
        Debug.Log("Level Started");
        parameters.ToList().ForEach(x => Debug.Log(x)); 
        
        _appMetrica.ReportEvent(StartLevelEventName, parameters);
        _appMetrica.SendEventsBuffer();

        SceneManager.sceneLoaded += OnLevelLoaded;
        BuildingsCounting.OnPlayerWin += OnLevelWin;
        BuildingsCounting.OnPlayerFail += OnLevelFail;
        
        _isLevelFinished = false;
    }
}
