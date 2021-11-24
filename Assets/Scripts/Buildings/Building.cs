using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(BuildingTeamSetting))]
    public class Building : MonoBehaviour
    {
        [SerializeField] private Team team;
        [SerializeField] private BuildingType type;
        [SerializeField] private List<Path.Path> attachedPaths;
        [SerializeField] private List<Building> availableBuildingsToPath;

        private BuildingTeamSetting _buildingTeam;
        private BuildingPathIndicating _indicating;
        private Vector3 _linePos;
        private const float LineYPos = 0.1f;

        private void Start()
        {
            _buildingTeam = GetComponent<BuildingTeamSetting>();
            _indicating = GetComponent<BuildingPathIndicating>();
            _linePos = transform.position;
            _linePos.y = LineYPos;
        }

        public Team GetTeam() => team;
        public Vector3 GetLinePos() => _linePos;
        public List<Building> GetAvailableBuildingsToPath() => availableBuildingsToPath;
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
                var enemy = TeamAssignment.GetInstance().GetEnemyByTeam(teamToChange);
                enemy.RemoveBuildingFromOwn(this);
                
                team = teamToChange;
                _buildingTeam.ChangeTeamTo(teamToChange);
                
                enemy = TeamAssignment.GetInstance().GetEnemyByTeam(teamToChange);
                enemy.AddBuildingToOwn(this);
            }
            else
            {
                team = teamToChange;
                _buildingTeam.ChangeTeamTo(teamToChange);
            }
            

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
