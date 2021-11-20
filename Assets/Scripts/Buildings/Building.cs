using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(BuildingTeamSetting))]
    public class Building : MonoBehaviour
    {
        [SerializeField] private Team team;
        [SerializeField] private int availablePathsCount = 1;
        [SerializeField] private int pathsCount;
        [SerializeField] private List<BuildingPathIndicatorsGroup> pathIndicators;

        private BuildingTeamSetting _buildingTeam;
        private Vector3 _linePos;
        private const float LineYPos = 0.1f;

        private void Start()
        {
            _buildingTeam = GetComponent<BuildingTeamSetting>();
            _linePos = transform.position;
            _linePos.y = LineYPos;
        }

        public Team GetTeam() => team;
        public Vector3 GetLinePos() => _linePos;
        
        public int GetAvailablePathsCount() => availablePathsCount;

        public void IncreaseAvailablePathsCount()
        {
            if (availablePathsCount > 3) return;
            
            pathIndicators[availablePathsCount - 1].ClearAllCircles();
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(false);
            
            availablePathsCount++;
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(true);
            pathIndicators[availablePathsCount - 1].SetColorForCircles(pathsCount, team);
        }
        public void DecreaseAvailablePathsCount()
        {
            if (availablePathsCount <= 1) return;
            
            pathIndicators[availablePathsCount - 1].ClearAllCircles();
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(false);
            
            availablePathsCount--;
            pathIndicators[availablePathsCount - 1].gameObject.SetActive(true);
            pathIndicators[availablePathsCount - 1].SetColorForCircles(pathsCount, team);
        }

        public void IncreasePathsCount()
        {
            if (pathsCount > availablePathsCount) return;
            
            pathsCount++;
            pathIndicators[availablePathsCount - 1].SetCircleColor(pathsCount - 1, team);
        }


        public void DecreasePathsCount()
        {
            if (pathsCount < 0) return;
            
            pathIndicators[availablePathsCount - 1].ClearCircleColor(pathsCount - 1);
            pathsCount--;
        }

        public void ChangeBuildingTeam(Team teamToChange)
        {
            team = teamToChange;
            _buildingTeam.ChangeTeamTo(teamToChange);
            //Particles
        }

        public bool IsPathCreationAvailable() => pathsCount < availablePathsCount;
    }
}
