using TMPro;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(Building))]
    public class BuildingUnitCount : MonoBehaviour
    {
        [SerializeField] private int unitCount = 10;
        [SerializeField] private int maxUnitCount = 65;
        [SerializeField] private TextMeshPro countText;

        private Building _building;
        private const int UnitLayer = 7;
        
        private void Start() => _building = GetComponent<Building>();
        private void Update() => countText.text = unitCount.ToString();

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
