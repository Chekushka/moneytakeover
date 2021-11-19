using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamColors : MonoBehaviour
{
    [SerializeField] private List<Material> buildingMaterials;
    [SerializeField] private List<Material> lineMaterials;

    private static TeamColors _instance;

    private void Awake()
    {
        if (_instance == null) 
            _instance = this; 
        else if(_instance == this)
            Destroy(gameObject);
    }
    
    public static TeamColors GetInstance() => _instance; 
    public Material GetBuildingMaterial(Team team) => buildingMaterials[(int)team];
    public Material GetLineMaterial(Team team) => lineMaterials[(int)team];
}