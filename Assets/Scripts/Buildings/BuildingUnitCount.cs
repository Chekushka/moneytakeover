using System;
using TMPro;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(Building), typeof(BuildingPathIndicating), 
        typeof(BuildingGrowing))]
    public class BuildingUnitCount : MonoBehaviour
    {
        [SerializeField] private int unitCount = 10;
        [SerializeField] private int maxUnitCount = 65;
        [SerializeField] private TextMeshPro countText;

        private Building _building;
        private BuildingPathIndicating _pathIndicating;
        private BuildingGrowing _buildingGrowing;
        private const int UnitLayer = 7;

        private void Start()
        {
            _building = GetComponent<Building>();
            _pathIndicating = GetComponent<BuildingPathIndicating>();
            _buildingGrowing = GetComponent<BuildingGrowing>();
        }

        private void Update()
        {
            if(unitCount < maxUnitCount)
                countText.text = unitCount.ToString();
            if (unitCount == maxUnitCount)
                countText.text = "MAX";
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer != UnitLayer) return;
            var unit = other.gameObject.GetComponent<Unit>();

            if(unit.startBuilding.GetInstanceID() == _building.GetInstanceID()) return;

            if (unitCount == 0 && unit.GetTeam() != _building.GetTeam())
                _building.ChangeBuildingTeam(unit.GetTeam());

            if (unit.GetTeam() == _building.GetTeam())
            {
                if (unitCount < maxUnitCount)
                {
                    if (unit.GetUnitType() == BuildingType.MonetaryYard)
                    {
                        if (unitCount == 18 || unitCount == 28)
                            _pathIndicating.IncreaseAvailablePathsCount();
                        unitCount += 2;
                    }
                    else
                    {
                        if (unitCount == 19 || unitCount == 29)
                            _pathIndicating.IncreaseAvailablePathsCount();
                        unitCount++;
                    }
                    
                    if(unitCount % 10 == 0 && unitCount >= 20)
                        _buildingGrowing.AddBuildingLevel();
                }
            }
            else
            {
                if (unit.GetUnitType() == BuildingType.Exchange)
                {
                    if (unitCount == 21 || unitCount == 31) 
                        _pathIndicating.DecreaseAvailablePathsCount();
                    unitCount -= 2;
                }
                else
                {
                    if (unitCount == 20 || unitCount == 30) 
                        _pathIndicating.DecreaseAvailablePathsCount();
                    unitCount--;
                }

                if(unitCount % 10 == 0 && unitCount >= 20)
                    _buildingGrowing.RemoveBuildingLevel();
            }
            
            Destroy(other.gameObject);
        }
    }
}
