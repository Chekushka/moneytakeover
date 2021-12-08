using Buildings;
using UnityEngine;

public class LevelEnding : MonoBehaviour
{
    private GameUILoading _gameUILoading;
    private void Start()
    {
        _gameUILoading = FindObjectOfType<GameUILoading>();
        
        BuildingsCounting.OnPlayerFail += ActivateLoseScreen;
        BuildingsCounting.OnPlayerWin += ActivateWinScreen;
    }

    private void ActivateWinScreen() => _gameUILoading.GetWinScreen().SetActive(true);
    private void ActivateLoseScreen() => _gameUILoading.GetFailScreen().SetActive(true);
}
