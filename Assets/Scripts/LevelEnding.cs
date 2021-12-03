using Buildings;
using UnityEngine;

public class LevelEnding : MonoBehaviour
{
    private FinalScreen _finalScreen;
    private void Start()
    {
        _finalScreen = FindObjectOfType<FinalScreen>();
        
        BuildingsCounting.OnPlayerFail += ActivateLoseScreen;
        BuildingsCounting.OnPlayerWin += ActivateWinScreen;
    }

    private void ActivateWinScreen() => _finalScreen.GetWinScreen().SetActive(true);
    private void ActivateLoseScreen() => _finalScreen.GetFailScreen().SetActive(true);
}
