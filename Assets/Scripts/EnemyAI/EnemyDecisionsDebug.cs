using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Paths;
using UnityEngine;

namespace EnemyAI
{
    public class EnemyDecisionsDebug : MonoBehaviour
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
            StartCoroutine(Decide(startDecisionTime));
        }

        private void Update()
        {
            if (buildingsOnOwn == null)
            {
                _isDefeated = true;
                Debug.Log("я пагіб");
            }
        }

        public void AddBuildingToOwn(Building building) => buildingsOnOwn.Add(building);
        public void RemoveBuildingFromOwn(Building building) => buildingsOnOwn.Remove(
            buildingsOnOwn.Find(x => x.GetInstanceID() == building.GetInstanceID()));

        private List<Building> FindTeamBuildings()
        {
            var buildingList = BuildingsCounting.GetInstance().buildings;
            return buildingList.Where(building => building.GetTeam() == team).ToList();
        }

        private IEnumerator Decide(float decisionTime)
        {
            yield return new WaitForSeconds(decisionTime);
            Debug.Log("я думаю");
            var availableBuildings = buildingsOnOwn.Where(building =>
                building.GetComponent<BuildingPathIndicating>().IsPathCreationAvailable()).ToList();
            Debug.Log("знайшов доступні " + availableBuildings.Count);
            if (availableBuildings.Count != 0)
            {
                var startBuilding = availableBuildings[Random.Range(0, availableBuildings.Count)];
                Debug.Log("додав початкову точку");
                var availableBuildingToMove = startBuilding.GetAvailableBuildingsToPath();
                Debug.Log("знайшов доступні для ходу");
                var endBuilding = availableBuildingToMove[Random.Range(0, availableBuildingToMove.Count)];
                Debug.Log("додав кінцеву точку");
                
                if (_pathCreating.GetPathByPoints(startBuilding, endBuilding) != null)
                {
                    Debug.Log("думаю чи видаляти");
                    if (Random.Range(0, 4) == 4)
                    {
                        _pathCreating.RemovePath(_pathCreating.GetPathByPoints(startBuilding, endBuilding));
                        Debug.Log("видалив");
                    }
                    else
                        Debug.Log("передумав видаляти");
                }
                else
                {
                    _pathCreating.CreatePath(startBuilding, endBuilding, team);
                    Debug.Log("я надумав додати");
                }
            }
            else
            {
                Debug.Log("німа доступних");
                Debug.Log("думаю чи видаляти");
                if (Random.Range(0, 2) == 2)
                {
                    _pathCreating.RemovePath(buildingsOnOwn[Random.Range(0, 
                        buildingsOnOwn.Count)].GetAttachedPaths()[Random.Range(0, buildingsOnOwn.Count)]);
                    Debug.Log("видалив");
                }
                else
                    Debug.Log("передумав видаляти");
                
            }
            
            if (!_isDefeated)
                StartCoroutine(Decide(Random.Range(minDecisionTime, maxDecisionTime)));
        }
    
    }
}
