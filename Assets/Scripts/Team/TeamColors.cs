using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamColors : MonoBehaviour
{
    [SerializeField] private List<Material> mainBuildingMaterials;
    [SerializeField] private List<Material> buildingNameMaterials;
    [SerializeField] private List<Color> buildingPathsCountIndicatorColors;
    [SerializeField] private List<Material> lineMaterials;
    [SerializeField] private List<Unit> unitsPrefabs;
    [SerializeField] private List<Unit> unitsMonYardPrefabs;
    [SerializeField] private List<Unit> unitsExchangePrefabs;

    private static TeamColors _instance;

    private void Awake()
    {
        if (_instance == null) 
            _instance = this; 
        else if(_instance == this)
            Destroy(gameObject);
    }
    
    public static TeamColors GetInstance() => _instance; 
    public Material GetBuildingMainMaterial(Team team) => mainBuildingMaterials[(int)team];
    public Material GetBuildingNameMaterial(Team team) => buildingNameMaterials[(int)team];
    public Material GetLineMaterial(Team team) => lineMaterials[(int)team];
    public Unit GetUnitPrefab(Team team) => unitsPrefabs[(int)team];
    public Unit GetMonYardUnitPrefab(Team team) => unitsMonYardPrefabs[(int)team];
    public Unit GetExchangeUnitPrefab(Team team) => unitsExchangePrefabs[(int)team];
    public Color GetBuildingPathsCountIndicatorColor(Team team) => buildingPathsCountIndicatorColors[(int)team];
}