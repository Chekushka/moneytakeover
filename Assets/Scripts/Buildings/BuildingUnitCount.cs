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
        private void Start()
        {
            _building = GetComponent<Building>();
        }

        private void Update()
        {
            countText.text = unitCount.ToString();

            if (unitCount % 10 == 1 && unitCount < 32)
                _building.IncreasePathsCount();
            if(unitCount % 10 == 9 && unitCount > 0)
                _building.DecreasePathsCount();
        }

        private void OnCollisionEnter(Collision other)
        {
            if(!other.gameObject.CompareTag(UnitTag)) return;
            var unit = other.gameObject.GetComponent<Unit>();

            if (unit.GetTeam() == _building.GetTeam())
                unitCount++;
            else
                unitCount--;
      
            Destroy(other.gameObject);
        }
    }
}
