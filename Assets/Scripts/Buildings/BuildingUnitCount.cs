using TMPro;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(Building), typeof(BuildingPathIndicating))]
    public class BuildingUnitCount : MonoBehaviour
    {
        [SerializeField] private int unitCount = 10;
        [SerializeField] private int maxUnitCount = 65;
        [SerializeField] private TextMeshPro countText;

        private Building _building;
        private BuildingPathIndicating _pathIndicating;
        private const int UnitLayer = 7;

        private void Start()
        {
            _building = GetComponent<Building>();
            _pathIndicating = GetComponent<BuildingPathIndicating>();
        }
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
                        _pathIndicating.IncreaseAvailablePathsCount();
                }
            }
            else
            {
                unitCount--;
                if (unitCount == 19 || unitCount == 29) 
                    _pathIndicating.DecreaseAvailablePathsCount();
            }
            
            Destroy(other.gameObject);
        }
    }
}
