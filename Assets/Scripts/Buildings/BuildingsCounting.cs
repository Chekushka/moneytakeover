using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class BuildingsCounting : MonoBehaviour
    {
        public delegate void PlayerFail();
        public static event PlayerFail OnPlayerFail;
        
        public delegate void PlayerWin();
        public static event PlayerWin OnPlayerWin;
        
        public List<Building> buildings;
        private static BuildingsCounting _instance;

        private void Awake()
        {
            if (_instance == null) 
                _instance = this; 
            else if(_instance == this)
                Destroy(gameObject);

            buildings = BuildingsToList();
        }
        
        public static BuildingsCounting GetInstance() => _instance; 

        private List<Building> BuildingsToList() => GetComponentsInChildren<Building>().ToList();
        
        public void CheckPlayerWinOrFail()
        {
            var playerBuildings = buildings.Where(building => building.GetTeam() ==
                                                              TeamAssignment.GetInstance().GetPlayerTeam()).ToList();
            if(playerBuildings.Count == 0)
                OnPlayerFail?.Invoke();
            if(playerBuildings.Count == buildings.Count)
                OnPlayerWin?.Invoke();
        }
    }
}
