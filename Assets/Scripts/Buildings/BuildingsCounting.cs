using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class BuildingsCounting : MonoBehaviour
    {
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
    }
}
