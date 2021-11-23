using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class BuildingTeamSetting : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> mainMaterialRenderers;
        [SerializeField] private List<MeshRenderer> buildingNameRenderers;
        [SerializeField] private List<GameObject> moneySigns;

        private Building _building;
        private GameObject _currentMoneySign;

        private void Start()
        {
            _building = GetComponent<Building>();
            
            _currentMoneySign = moneySigns[(int)_building.GetTeam()];
            _currentMoneySign.SetActive(true);
            
            ChangeTeamTo(_building.GetTeam());
        }

        public void ChangeTeamTo(Team team)
        {
            foreach (var rendererComponent in mainMaterialRenderers)
                rendererComponent.material = TeamColors.GetInstance().GetBuildingMainMaterial(team);
            foreach (var rendererComponent in buildingNameRenderers)
                rendererComponent.material = TeamColors.GetInstance().GetBuildingNameMaterial(team);
            
            _currentMoneySign.SetActive(false);
            _currentMoneySign = moneySigns[(int)team];
            _currentMoneySign.SetActive(true);
        }
    }
}
