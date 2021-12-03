using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(Building), typeof(BuildingPathIndicating), 
        typeof(BuildingGrowing))]
    public class BuildingUnitCount : MonoBehaviour
    {
        [SerializeField] private int unitCount = 10;
        [SerializeField] private TextMeshPro countText;

        private Building _building;
        private BuildingPathIndicating _pathIndicating;
        private BuildingGrowing _buildingGrowing;
        private int _maxUnitCount;
        private int _lastUnitCount;
        private float timeToIncreaseUnitCount = 7;
        private const int UnitLayer = 7;

        private void Start()
        {
            _building = GetComponent<Building>();
            _pathIndicating = GetComponent<BuildingPathIndicating>();
            _buildingGrowing = GetComponent<BuildingGrowing>();
            _maxUnitCount = BuildingsCounting.GetInstance().GetMaxUnitCount();
            
            SetStartBuildingCount();
            StartCoroutine(IncreaseUnitCount());
        }

        private void FixedUpdate()
        {
            if(unitCount < _maxUnitCount)
                countText.text = unitCount.ToString();
            if (unitCount >= _maxUnitCount)
                countText.text = "MAX";

            if (unitCount % 10 == 0 && unitCount < _maxUnitCount)
            {
                if (unitCount >= 10)
                {
                    if (unitCount > _lastUnitCount)
                        if(unitCount < 30)
                            _pathIndicating.IncreaseAvailablePathsCount();
                    
                    if (unitCount < _lastUnitCount)
                        _pathIndicating.DecreaseAvailablePathsCount();
                }

                if (unitCount > 10)
                {
                    if (unitCount > _lastUnitCount)
                        _buildingGrowing.AddBuildingLevel();
                    if (unitCount < _lastUnitCount)
                        _buildingGrowing.RemoveBuildingLevel();
                }
            }

            _lastUnitCount = unitCount;
        }

        public bool IsCountMax() => unitCount >= _maxUnitCount;
        public int GetUnitCount() => unitCount;
        public void ResetUnitCount() => StartCoroutine(IncreaseUnitCount());
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer != UnitLayer) return;
            var unit = other.gameObject.GetComponent<Unit>();

            if(unit.startBuilding.GetInstanceID() == _building.GetInstanceID()) return;

            if (unitCount == 0 && unit.GetTeam() != _building.GetTeam())
                _building.ChangeBuildingTeam(unit.GetTeam());

            if (unit.GetTeam() == _building.GetTeam())
            {
                if (unitCount < _maxUnitCount)
                {
                    if (unit.GetUnitType() == BuildingType.MonetaryYard)
                        unitCount += 2;
                    else
                        unitCount++;
                }
            }
            else
            {
                if (unit.GetUnitType() == BuildingType.Exchange)
                    unitCount -= 2;
                else
                    unitCount--;
            }
            
            Destroy(other.gameObject);
        }

        private void SetStartBuildingCount()
        {
            _lastUnitCount = unitCount;
            if (unitCount > _maxUnitCount) return;

            var growMultiplier = 0;
            if (unitCount >= 10)
            {
                if (unitCount >= 10 && unitCount < 20)
                    growMultiplier = 2;
                if (unitCount >= 20)
                    growMultiplier = 3;
                for (var i = 1; i < growMultiplier; i++)
                    _pathIndicating.IncreaseAvailablePathsCount();
            }

            if (unitCount > 10)
            {
                growMultiplier = unitCount / 10;
                for (var i = 1; i < growMultiplier; i++)
                    _buildingGrowing.AddBuildingLevel();
            }
        }

        private IEnumerator IncreaseUnitCount()
        {
            yield return new WaitForSeconds(timeToIncreaseUnitCount);
            
            if(_building.GetTeam() != Team.Neutral)
                unitCount++;
            if (unitCount < _maxUnitCount)
                StartCoroutine(IncreaseUnitCount());
        }
    }
}
