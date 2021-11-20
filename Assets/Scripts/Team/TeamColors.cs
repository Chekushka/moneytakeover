using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamColors : MonoBehaviour
{
    [SerializeField] private List<Material> buildingMaterials;
    [SerializeField] private List<Color> buildingPathsCountIndicatorColors;
    [SerializeField] private List<Material> lineMaterials;
    [SerializeField] private List<Unit> unitsPrefabs;

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
    public Unit GetUnitPrefab(Team team) => unitsPrefabs[(int)team];
    public Color GetBuildingPathsCountIndicatorColor(Team team) => buildingPathsCountIndicatorColors[(int)team];
}