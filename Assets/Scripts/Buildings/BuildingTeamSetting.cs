using UnityEngine;

namespace Buildings
{
    public class BuildingTeamSetting : MonoBehaviour
    {
        private Building _building;
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _building = GetComponent<Building>();
            _meshRenderer = GetComponent<MeshRenderer>();

            ChangeTeamTo(_building.GetTeam());
        }

        public void ChangeTeamTo(Team team) => _meshRenderer.material = 
            TeamColors.GetInstance().GetBuildingMaterial(team);
    }
}
