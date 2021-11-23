using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(BuildingTeamSetting))]
    public class Building : MonoBehaviour
    {
        [SerializeField] private Team team;
        [SerializeField] private BuildingType type;

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

        public void ChangeBuildingTeam(Team teamToChange)
        {
            team = teamToChange;
            _buildingTeam.ChangeTeamTo(teamToChange);
            //Particles
        }
    }

    public enum BuildingType
    {
        Bank,
        MonetaryYard,
        Exchange
    }
}
