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
                    if (unitCount == 21 || unitCount == 31)
                        _pathIndicating.IncreaseAvailablePathsCount();
                    
                    if(unitCount % 10 == 1 && unitCount > 20)
                        _buildingGrowing.AddBuildingLevel();
                }
            }
            else
            {
                unitCount--;
                if (unitCount == 19 || unitCount == 29) 
                    _pathIndicating.DecreaseAvailablePathsCount();
                if(unitCount % 10 == 9 && unitCount > 10)
                    _buildingGrowing.RemoveBuildingLevel();
            }
            
            Destroy(other.gameObject);
        }
    }
}
