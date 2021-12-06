using System;
using System.Collections.Generic;
using Paths;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(Building))]
    public class BuildingPathIndicating : MonoBehaviour
    {
        [SerializeField] private int availablePathsCount = 1;
        [SerializeField] private int pathsCount;
        [SerializeField] private List<BuildingPathIndicatorsGroup> pathIndicators;
        [SerializeField] private SpriteRenderer indicatorsTextPanelBackground;

        private Building _building;
        private BuildingUnitCount _unitCount;
        private PathCreating _pathCreating;
        private int _lastAvailablePathCount;

        private void Start()
        {
            _building = GetComponent<Building>();
            _unitCount = GetComponent<BuildingUnitCount>();
            _pathCreating = FindObjectOfType<PathCreating>();
            indicatorsTextPanelBackground.color = 
                TeamColors.GetInstance().GetBuildingPathsCountIndicatorColor(_building.GetTeam());
            _lastAvailablePathCount = 1;
        }

        private void FixedUpdate()
        {
            var unitCount = _unitCount.GetUnitCount();
           
            if (unitCount >= 0 && unitCount < 10)
                SetAvailablePathsCount(1);
            else if (unitCount >= 10 && unitCount < 20)
                SetAvailablePathsCount(2);
            else if (unitCount >= 20)
                SetAvailablePathsCount(3);
            
            if(pathsCount < 4 && pathsCount > 0)
                pathIndicators[availablePathsCount - 1].ColorCircles(pathsCount, _building.GetTeam());
        }

        public int GetAvailablePathsCount() => availablePathsCount;
        public bool IsPathCreationAvailable() => pathsCount < availablePathsCount;

        public void ResetPathCountIndicating()
        {
            foreach (var indicator in pathIndicators)
                indicator.ClearAllCircles();
            availablePathsCount = 1;
            pathsCount = 0;

            indicatorsTextPanelBackground.color = 
                TeamColors.GetInstance().GetBuildingPathsCountIndicatorColor(_building.GetTeam());
        }

        private void SetAvailablePathsCount(int pathCount)
        {
            if (availablePathsCount >= 4 || availablePathsCount <= 0) return;
            
            availablePathsCount = pathCount;
            if(availablePathsCount == _lastAvailablePathCount) return;
            
            if(_lastAvailablePathCount > availablePathsCount 
               && _building.GetAttachedPaths().Count >= availablePathsCount && _building.GetAttachedPaths().Count > 0)
            {
                var pathToRemove = _building.GetAttachedPaths()[_building.GetAttachedPaths().Count - 1];
                if (pathToRemove.IsPathInBattle())
                    _pathCreating.CreateLineAfterBattle(pathToRemove);
                else
                    _pathCreating.RemovePath(pathToRemove);
            }
            
            pathIndicators[_lastAvailablePathCount - 1].ClearAllCircles();
            pathIndicators[_lastAvailablePathCount - 1].gameObject.SetActive(false);
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(true);

            _lastAvailablePathCount = availablePathsCount;
        }

        public void IncreasePathsCount()
        {
            if (pathsCount > availablePathsCount) return;
            pathsCount++;
        }


        public void DecreasePathsCount()
        {
            if (pathsCount <= 0) return;
            pathsCount--;
        }
    }
}
