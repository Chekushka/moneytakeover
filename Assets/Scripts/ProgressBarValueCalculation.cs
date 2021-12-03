using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarValueCalculation : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    private void Start() => progressBar.value = CalculateProgressBarValue() / 100;
    private void Update() => progressBar.value = CalculateProgressBarValue() / 100;

    private float CalculateProgressBarValue()
    {
        var playerUnitsCount = CountCurrentUnitCountInBuildings(GetPlayerBuildings());
        var enemiesUnitsCount = CountCurrentUnitCountInBuildings(GetEnemiesBuildings());

        var value = 0;
        
        if(enemiesUnitsCount != 0 || playerUnitsCount != 0)
            value = playerUnitsCount * 100 / (playerUnitsCount + enemiesUnitsCount);
        
        return value;
    }

    private int CountCurrentUnitCountInBuildings(List<Building> buildings)
    {
        var count = 0;

        foreach (var building in buildings)
        {
            if (building.GetUnitCount() < 10)
                count += building.GetUnitCount();
            else
                count += 10;
        }
        return count;
    }

    private List<Building> GetEnemiesBuildings()
    {
        return BuildingsCounting.GetInstance().buildings.Where(building =>
            building.GetTeam() != TeamAssignment.GetInstance().GetPlayerTeam() 
            && building.GetTeam() != Team.Neutral).ToList();
    }

    private List<Building> GetPlayerBuildings()
    {
        return BuildingsCounting.GetInstance().buildings.Where(building =>
            building.GetTeam() == TeamAssignment.GetInstance().GetPlayerTeam()).ToList();
    }
}
