using System;
using System.Collections.Generic;
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

        private void Start()
        {
            _building = GetComponent<Building>();
            indicatorsTextPanelBackground.color = 
                TeamColors.GetInstance().GetBuildingPathsCountIndicatorColor(_building.GetTeam());
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

        public void IncreaseAvailablePathsCount()
        {
            if (availablePathsCount > 3) return;
            
            pathIndicators[availablePathsCount - 1].ClearAllCircles();
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(false);
            
            availablePathsCount++;
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(true);
            pathIndicators[availablePathsCount - 1].SetColorForCircles(pathsCount, _building.GetTeam());
        }
        
        public void DecreaseAvailablePathsCount()
        {
            if (availablePathsCount <= 1) return;
            
            pathIndicators[availablePathsCount - 1].ClearAllCircles();
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(false);
            
            availablePathsCount--;
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(true);
            pathIndicators[availablePathsCount - 1].SetColorForCircles(pathsCount, _building.GetTeam());
        }

        public void IncreasePathsCount()
        {
            if (pathsCount > availablePathsCount) return;
            
            pathsCount++;
            pathIndicators[availablePathsCount - 1].SetCircleColor(pathsCount - 1, _building.GetTeam());
        }


        public void DecreasePathsCount()
        {
            if (pathsCount < 0) return;
            
            pathIndicators[availablePathsCount - 1].ClearCircleColor(pathsCount - 1);
            pathsCount--;
        }
    }
}
