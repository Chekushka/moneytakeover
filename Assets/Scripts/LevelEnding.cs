using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

public class LevelEnding : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    private void Start()
    {
        BuildingsCounting.OnPlayerFail += ActivateLoseScreen;
        BuildingsCounting.OnPlayerWin += ActivateWinScreen;
    }

    private void ActivateWinScreen() => winScreen.SetActive(true);
    private void ActivateLoseScreen() => loseScreen.SetActive(true);
}
