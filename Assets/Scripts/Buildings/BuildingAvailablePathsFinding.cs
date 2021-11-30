using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class BuildingAvailablePathsFinding : MonoBehaviour
    {
        private Building _building;

        private void Start()
        {
            _building = GetComponent<Building>();
            _building.SetAvailableBuildingToPath(FindAvailableBuildings());
        }

        private List<Building> FindAvailableBuildings()
        {
            var buildingsOnScene = BuildingsCounting.GetInstance().buildings;
            var availableBuildings = new List<Building>();

            foreach (var building in buildingsOnScene)
            {
                var startPos = _building.GetLinePos();
                var endPos = building.GetLinePos();
                var distance = Vector3.Distance(startPos, endPos);
                var direction = (endPos - startPos).normalized;

                if (building.GetInstanceID() != _building.GetInstanceID())
                {
                    var hits = 
                        Physics.RaycastAll(startPos, direction, distance);
                    
                    if(hits.Length == 1)
                        availableBuildings.Add(building);
                }
            }

            return availableBuildings;
        }
    }
}
