using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Path;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyAI
{
    public class EnemyDecisions : MonoBehaviour
    {
        [SerializeField] private Team team;
        [SerializeField] private List<Building> buildingsOnOwn;
        [SerializeField] private float startDecisionTime = 5f;
        [SerializeField] private float minDecisionTime = 3f;
        [SerializeField] private float maxDecisionTime = 10f;
        
        private PathCreating _pathCreating;
        private bool _isDefeated;
        private void Start()
        {
            _pathCreating = FindObjectOfType<PathCreating>();   
            buildingsOnOwn = FindTeamBuildings();
            StartCoroutine(MakeDecision(startDecisionTime));
        }

        private void Update()
        {
            if (buildingsOnOwn == null)
                _isDefeated = true;
        }

        public void AddBuildingToOwn(Building building) => buildingsOnOwn.Add(building);
        public void RemoveBuildingFromOwn(Building building) => buildingsOnOwn.Remove(
            buildingsOnOwn.Find(x => x.GetInstanceID() == building.GetInstanceID()));

        private List<Building> FindTeamBuildings()
        {
            var buildingList = BuildingsCounting.GetInstance().buildings;
            return buildingList.Where(building => building.GetTeam() == team).ToList();
        }

        private IEnumerator MakeDecision(float decisionTime)
        {
            yield return new WaitForSeconds(decisionTime);
            var availableBuildings = buildingsOnOwn.Where(building =>
                building.GetComponent<BuildingPathIndicating>().IsPathCreationAvailable()).ToList();
            
            if (availableBuildings.Count != 0)
            {
                var startBuilding = availableBuildings[Random.Range(0, availableBuildings.Count)];
                var availableBuildingToMove = startBuilding.GetAvailableBuildingsToPath();
                var endBuilding = availableBuildingToMove[Random.Range(0, availableBuildingToMove.Count)];

                if (_pathCreating.GetPathByPoints(startBuilding, endBuilding) != null)
                {
                    if (Random.Range(0, 4) == 4)
                        _pathCreating.RemovePath(_pathCreating.GetPathByPoints(startBuilding, endBuilding));
                    else
                        _pathCreating.CreatePath(startBuilding, endBuilding, team); 
                }
            }
            else if (Random.Range(0, 2) == 2)
                    _pathCreating.RemovePath(buildingsOnOwn[Random.Range(0, 
                        buildingsOnOwn.Count)].GetAttachedPaths()[Random.Range(0, buildingsOnOwn.Count)]);

            if (!_isDefeated)
                StartCoroutine(MakeDecision(Random.Range(minDecisionTime, maxDecisionTime)));
        }
    }
}