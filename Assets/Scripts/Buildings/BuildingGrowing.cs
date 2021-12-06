using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class BuildingGrowing : MonoBehaviour
    {
        [SerializeField] private List<GameObject> buildingLevels;
        [SerializeField] private GameObject endLevel;
        [SerializeField] private MeshRenderer levelMesh;

        private List<GameObject> _activeBuildingLevels;
        private BuildingUnitCount _unitCount;
        private Vector3 _startRoofPos;
        private int _currentLevel;
        private int _lastSetLevel;

        private void Start()
        {
            _activeBuildingLevels = new List<GameObject>();
            _unitCount = GetComponent<BuildingUnitCount>();
            _lastSetLevel = _currentLevel;
            _startRoofPos = endLevel.transform.position;
        }

        private void Update()
        {
            var unitCount = _unitCount.GetUnitCount();

            if (unitCount > 0 && unitCount < 10)
                SetBuildingLevel(0);
            else if (unitCount >= 20 && unitCount < 30)
                SetBuildingLevel(1);
            else if (unitCount >= 30 && unitCount < 40)
                SetBuildingLevel(2);
            else if (unitCount >= 40 && unitCount < 50)
                SetBuildingLevel(3);
            else if (unitCount >= 50)
                SetBuildingLevel(4);
        }

        private void SetBuildingLevel(int buildingLevel)
        {
            if (buildingLevel == _currentLevel) return;
            _currentLevel = buildingLevel;

            if (_currentLevel == 0)
                SetGrowingToDefaultState();
            else
            {
                SetGrowingToDefaultState();
                
                for (var i = 0; i < _currentLevel; i++)
                {
                    var levelPos = buildingLevels[i].transform.position;
                    Vector3 newLevelPos;
                    if (i != 0)
                    {
                        levelPos = buildingLevels[i - 1].transform.position;
                        newLevelPos = new Vector3(levelPos.x, levelPos.y + levelMesh.bounds.size.y,
                            levelPos.z);
                    }
                    else
                        newLevelPos = levelPos;
                    
                    buildingLevels[i].transform.position = newLevelPos;
                    buildingLevels[i].SetActive(true);
                }

                var endLevelPos = endLevel.transform.position;
                var newPos = new Vector3(endLevelPos.x, endLevelPos.y + levelMesh.bounds.size.y * _currentLevel - 1,
                    endLevelPos.z);
                endLevel.transform.position = newPos;

                for (var i = 0; i < _currentLevel; i++)
                    _activeBuildingLevels.Add(buildingLevels[i]);
            }

            _lastSetLevel = _currentLevel;
        }

        private void SetGrowingToDefaultState()
        {
            endLevel.transform.position = _startRoofPos;
            foreach (var level in buildingLevels)
            {
                level.SetActive(false);
                level.transform.position = _startRoofPos;
                _activeBuildingLevels.Remove(buildingLevels.Find(x => x.GetInstanceID()
                                                                      == level.GetInstanceID()));
            }
        }
    }
}