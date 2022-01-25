using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class BuildingsCounting : MonoBehaviour
    {
        [SerializeField] private int maxBuildingsUnitCount = 65;
        public List<Building> buildings;
        private static BuildingsCounting _instance;

        public delegate void PlayerFail();
        public static event PlayerFail OnPlayerFail;
        
        public delegate void PlayerWin();
        public static event PlayerWin OnPlayerWin;

        private void Awake()
        {
            if (_instance == null) 
                _instance = this; 
            else if(_instance == this)
                Destroy(gameObject);

            buildings = BuildingsToList();
        }

        private void Update() => CheckPlayerWinOrFail();

        public static BuildingsCounting GetInstance() => _instance;

        public int GetMaxUnitCount() => maxBuildingsUnitCount;

        private List<Building> BuildingsToList() => GetComponentsInChildren<Building>().ToList();
        
        private void CheckPlayerWinOrFail()
        {
            var playerBuildings = buildings.Where(building => building.GetTeam() ==
                                                              TeamAssignment.GetInstance().GetPlayerTeam()).ToList();
            if (playerBuildings.Count == 0)
                OnPlayerFail?.Invoke();
            if (playerBuildings.Count == buildings.Count)
                OnPlayerWin?.Invoke();
        }
    }
}
