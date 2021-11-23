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
        private int _currentLevel;

        private void Start()
        {
            _activeBuildingLevels = new List<GameObject>();
        }

        public void AddBuildingLevel()
        {
            if (_currentLevel >= buildingLevels.Count) return;

            buildingLevels[_currentLevel].SetActive(true);

            foreach (var level in _activeBuildingLevels)
            {
                var position = level.transform.position;
                var newPosition = new Vector3(position.x, position.y + levelMesh.bounds.size.y,
                    position.z);
                level.transform.position = newPosition;
            }

            var endLevelPos = endLevel.transform.position;
            var newPos = new Vector3(endLevelPos.x, endLevelPos.y + levelMesh.bounds.size.y,
                endLevelPos.z);
            endLevel.transform.position = newPos;

            _activeBuildingLevels.Add(buildingLevels[_currentLevel]);
            _currentLevel++;
        }
        public void RemoveBuildingLevel()
        {
            buildingLevels[_currentLevel].SetActive(false);

            foreach (var level in _activeBuildingLevels)
            {
                var position = level.transform.position;
                var newPosition = new Vector3(position.x, position.y - levelMesh.bounds.size.y,
                    position.z);
                level.transform.position = newPosition;
            }

            var endLevelPos = endLevel.transform.position;
            var newPos = new Vector3(endLevelPos.x, endLevelPos.y - levelMesh.bounds.size.y,
                endLevelPos.z);
            endLevel.transform.position = newPos;

            _activeBuildingLevels.Remove(buildingLevels[_currentLevel]);
            _currentLevel--;
        }
    }
}
