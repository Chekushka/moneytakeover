using System;
using System.Collections.Generic;
using System.Linq;
using EnemyAI;
using Path;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(BuildingTeamSetting), typeof(BuildingPathIndicating), 
        typeof(BuildingAvailablePathsFinding))]
    public class Building : MonoBehaviour
    {
        [SerializeField] private Team team;
        [SerializeField] private BuildingType type;
        [SerializeField] private List<Path.Path> attachedPaths;
        [SerializeField] private List<Building> availableBuildingsToPath;
        [SerializeField] private GameObject buildingChangeParticles;
        [SerializeField] private Transform particleSpawnPoint;
        
        public int unitSpawnPower;

        private BuildingTeamSetting _buildingTeam;
        private BuildingPathIndicating _indicating;
        private PathCreating _pathCreating;
        private Vector3 _linePos;
        private Collider _collider;
        private const float LineYPos = 0.1f;

        private void Awake()
        {
            _buildingTeam = GetComponent<BuildingTeamSetting>();
            _indicating = GetComponent<BuildingPathIndicating>();
            _collider = GetComponent<Collider>();
            _pathCreating = FindObjectOfType<PathCreating>();
            _linePos = transform.position;
            _linePos.y = LineYPos;
        }

        public Team GetTeam() => team;
        public BuildingType GetBuildingType() => type;
        public Vector3 GetLinePos() => _linePos;
        public List<Building> GetAvailableBuildingsToPath() => availableBuildingsToPath;
        public void SetAvailableBuildingToPath(List<Building> buildings) => availableBuildingsToPath = buildings;
        public Collider GetBuildingCollider() => _collider;
        public List<Path.Path> GetAttachedPaths() => attachedPaths;
        public void AttachPath(Path.Path path) => attachedPaths.Add(path);

        public void RemovePath(Path.Path path)
        {
            _indicating.DecreasePathsCount();
            var pathToRemove = attachedPaths.Find(x => x.GetInstanceID() == path.GetInstanceID());
            attachedPaths.Remove(pathToRemove);
            Destroy(pathToRemove.gameObject);
        }

        public void ChangeBuildingTeam(Team teamToChange)
        {
            foreach (var path in attachedPaths)
                Destroy(path.gameObject);
            attachedPaths.Clear();

            if (teamToChange != TeamAssignment.GetInstance().GetPlayerTeam())
            {
                if (team != TeamAssignment.GetInstance().GetPlayerTeam())
                {
                    EnemyDecisions currentPlayer;
                    if (team != Team.Neutral)
                    {
                        currentPlayer = TeamAssignment.GetInstance().GetEnemyByTeam(team);
                        currentPlayer.RemoveBuildingFromOwn(this); 
                    }

                    team = teamToChange;
                    _buildingTeam.ChangeTeamTo(teamToChange);

                    currentPlayer = TeamAssignment.GetInstance().GetEnemyByTeam(team);
                    currentPlayer.AddBuildingToOwn(this);
                }
                else
                {
                    team = teamToChange;
                    _buildingTeam.ChangeTeamTo(teamToChange);

                    var currentPlayer = TeamAssignment.GetInstance().GetEnemyByTeam(team);
                    currentPlayer.AddBuildingToOwn(this);
                }
            }
            else
            {
                if (team != Team.Neutral)
                {
                    var currentPlayer = TeamAssignment.GetInstance().GetEnemyByTeam(team);
                    currentPlayer.RemoveBuildingFromOwn(this); 
                }

                team = teamToChange;
                _buildingTeam.ChangeTeamTo(team);
            }
            
            _indicating.ResetPathCountIndicating();
            UpdateUnitSpawnPower();
            Instantiate(buildingChangeParticles, particleSpawnPoint.position, Quaternion.identity);
            
            BuildingsCounting.GetInstance().CheckPlayerWinOrFail();
        }

        public void UpdateUnitSpawnPower()
        {
            var pathsEndThis = _pathCreating.GetPathsList().Where(x => x.GetEndBuilding().
                gameObject.GetInstanceID() == gameObject.GetInstanceID() && x.GetPathTeam() == team).ToList();
            unitSpawnPower = pathsEndThis.Count;
        }
    }

    public enum BuildingType
    {
        Bank,
        MonetaryYard,
        Exchange
    }
}
