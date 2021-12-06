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
        [SerializeField] private int unitSpawnPowerOnMax = 4;
        [SerializeField] private TextMeshPro countText;

        private Building _building;
        private int _maxUnitCount;

        private const float TimeToIncreaseUnitCount = 4;
        private const int UnitLayer = 7;

        private void Start()
        {
            _building = GetComponent<Building>();
            _maxUnitCount = BuildingsCounting.GetInstance().GetMaxUnitCount();
            
            StartCoroutine(IncreaseUnitCount());
        }

        private void Update()
        {
            if (unitCount < _maxUnitCount && unitCount >= 0)
            {
                countText.text = unitCount.ToString();
                _building.UpdateUnitSpawnPower();
            }
            if (unitCount >= _maxUnitCount)
            {
                countText.text = "MAX";
                _building.unitSpawnPower = unitSpawnPowerOnMax;
            }
        }
        
        public int GetUnitCount() => unitCount;
        public void ResetUnitCount() => StartCoroutine(IncreaseUnitCount());
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer != UnitLayer) return;
            var unit = other.gameObject.GetComponent<Unit>();

            if(unit.GetTargetPos() != _building.GetLinePos()) return;
            if(unit.startBuilding.GetInstanceID() == _building.GetInstanceID()) return;

            if (unitCount <= 0 && unit.GetTeam() != _building.GetTeam())
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

        private IEnumerator IncreaseUnitCount()
        {
            yield return new WaitForSeconds(TimeToIncreaseUnitCount);
            
            if(_building.GetTeam() != Team.Neutral)
                unitCount++;    
            if (unitCount < _maxUnitCount)
                StartCoroutine(IncreaseUnitCount());
        }
    }
}
