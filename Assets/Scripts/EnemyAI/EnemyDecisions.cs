using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Paths;
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
            StartCoroutine(Decide(startDecisionTime));
        }

        private void Update()
        {
            if (buildingsOnOwn.Count == 0)
            {
                _isDefeated = true;
                Debug.Log(gameObject.name + ": " + "я пагіб");
                enabled = false;
            }
        }

        public bool IsDefeated() => _isDefeated;
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
            Debug.Log(gameObject.name + ": " + "я думаю");
            var availableBuildings = buildingsOnOwn.Where(building =>
                building.GetComponent<BuildingPathIndicating>().IsPathCreationAvailable()).ToList();
            Debug.Log(gameObject.name + ": " + "знайшов доступні " + availableBuildings.Count);
            if (availableBuildings.Count != 0)
            {
                var startBuilding = availableBuildings[Random.Range(0, availableBuildings.Count)];
                Debug.Log(gameObject.name + ": " + "додав початкову точку");
                var availableBuildingToMove = startBuilding.GetAvailableBuildingsToPath();
                Debug.Log(gameObject.name + ": " + "знайшов доступні для ходу");
                var endBuilding = availableBuildingToMove[Random.Range(0, availableBuildingToMove.Count)];
                Debug.Log(gameObject.name + ": " + "додав кінцеву точку");

                if (_pathCreating.GetPathByPoints(startBuilding, endBuilding) != null)
                {
                    Debug.Log(gameObject.name + ": " + "думаю чи видаляти");
                    if (Random.value >= 0.8)
                    {
                        var path = _pathCreating.GetPathByPoints(startBuilding, endBuilding);
                        
                        if(path.IsPathInBattle())
                            _pathCreating.CreateLineAfterBattle(path);
                        else
                            _pathCreating.RemovePath(path);
                        Debug.Log(gameObject.name + ": " + "видалив");
                    }
                    else
                        Debug.Log(gameObject.name + ": " + "передумав видаляти");
                }
                else
                {
                    _pathCreating.CreatePath(startBuilding, endBuilding, team);
                    Debug.Log(gameObject.name + ": " + "я надумав додати");
                }
            }
            else
            {
                Debug.Log(gameObject.name + ": " + "німа доступних");
                Debug.Log(gameObject.name + ": " + "думаю чи видаляти");
                if (Random.value >= 0.6)
                {
                    var path = buildingsOnOwn[Random.Range(0,
                        buildingsOnOwn.Count)].GetAttachedPaths()[Random.Range(0, buildingsOnOwn.Count)];
                        
                    if(path.IsPathInBattle())
                        _pathCreating.CreateLineAfterBattle(path);
                    else
                        _pathCreating.RemovePath(path);
                    Debug.Log(gameObject.name + ": " + "видалив");
                }
                else
                    Debug.Log(gameObject.name + ": " + "передумав видаляти");

            }

            if (!_isDefeated)
                StartCoroutine(Decide(Random.Range(minDecisionTime, maxDecisionTime)));
        }
    }
}
