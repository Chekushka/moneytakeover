using System.Collections;
using Buildings;
using UnityEngine;

namespace Paths
{
    [RequireComponent(typeof(Path))]
    public class PathUnitSpawning : MonoBehaviour
    { 
        private float _startDelayBetweenSpawn = 1f;
        private Path _unitPath;
        private Transform _unitParent;
        private float _currentDelay;
        private int _multiplier;
        private int _lastMultiplier;

        private void Start()
        {
            _unitPath = GetComponent<Path>();
            _unitParent = FindObjectOfType<UnitParent>().transform;
            _currentDelay = _startDelayBetweenSpawn;
            _lastMultiplier = _multiplier;
            StartCoroutine(SetSpawningDelay());
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

        private IEnumerator SetSpawningDelay()
        {
            _multiplier = _unitPath.GetStartBuilding().unitSpawnPower;
            if (_multiplier != _lastMultiplier)
            {
                _currentDelay = _startDelayBetweenSpawn;
                _currentDelay -= _multiplier * 0.1f;
                _lastMultiplier = _multiplier;
            }
            yield return new WaitForSeconds(_currentDelay);
            SpawnUnit();
            if(this != null)
                StartCoroutine(SetSpawningDelay());
        }
    }
}
