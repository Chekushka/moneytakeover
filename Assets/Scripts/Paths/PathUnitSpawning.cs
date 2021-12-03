using Buildings;
using UnityEngine;

namespace Paths
{
    [RequireComponent(typeof(Path))]
    public class PathUnitSpawning : MonoBehaviour
    {
        [SerializeField] private float timeToStartSpawn = 1f;
        [SerializeField] private float delayBetweenSpawn = 1f;
        
        private Path _unitPath;
        private Transform _unitParent;

        private void Start()
        {
            _unitPath = GetComponent<Path>();
            _unitParent = FindObjectOfType<UnitParent>().transform;
            InvokeRepeating(nameof(SpawnUnit), timeToStartSpawn, 
                delayBetweenSpawn - _unitPath.GetStartBuilding().unitSpawnPower * 0.2f);
        }

        private void SpawnUnit()
        {
            var unitStartPos = _unitPath.GetPathStartPos();
            var unitStartRotation = Quaternion.LookRotation(_unitPath.GetPathEndPos());
            Unit unitPrefab = null;
            var buildingType = _unitPath.GetStartBuilding().GetBuildingType();
            
            switch (buildingType)
            {
                case BuildingType.Bank:
                    unitPrefab = TeamColors.GetInstance().GetUnitPrefab(_unitPath.GetPathTeam());
                    break;
                case BuildingType.MonetaryYard:
                    unitPrefab = TeamColors.GetInstance().GetMonYardUnitPrefab(_unitPath.GetPathTeam());
                    break;
                case BuildingType.Exchange:
                    unitPrefab = TeamColors.GetInstance().GetExchangeUnitPrefab(_unitPath.GetPathTeam());
                    break;
            }
            
            var newUnit = Instantiate(unitPrefab, unitStartPos, unitStartRotation, _unitParent);
            newUnit.SetTargetPos(_unitPath.GetPathEndPos());
            newUnit.startBuilding = _unitPath.GetStartBuilding();
            
        }
    }
}
