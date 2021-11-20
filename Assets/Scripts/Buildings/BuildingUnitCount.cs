using System;
using TMPro;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(Building))]
    public class BuildingUnitCount : MonoBehaviour
    {
        [SerializeField] private int unitCount = 10;
        [SerializeField] private TextMeshPro countText;

        private Building _building;
        private const string UnitTag = "Unit";
        
        private void Start() => _building = GetComponent<Building>();
        private void Update() => countText.text = unitCount.ToString();

        private void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.CompareTag(UnitTag)) return;
            var unit = other.gameObject.GetComponent<Unit>();

            if(unit.startBuilding.GetInstanceID() == _building.GetInstanceID()) return;

            if (unitCount == 0 && unit.GetTeam() != _building.GetTeam())
                _building.ChangeBuildingTeam(unit.GetTeam());

            if (unit.GetTeam() == _building.GetTeam())
            {
                if (unitCount < 100)
                {
                    unitCount++;
                    if ((unitCount == 21 || unitCount == 31))
                        _building.IncreaseAvailablePathsCount();
                }
            }
            else
            {
                unitCount--;
                if (unitCount == 19 || unitCount == 29) 
                    _building.DecreaseAvailablePathsCount();
            }
            
            Destroy(other.gameObject);
        }
    }
}
