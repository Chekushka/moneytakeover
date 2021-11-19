using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Team team;
        [SerializeField] private int availablePathsCount = 1;

        private Vector3 _linePos;
        private const float LineYPos = 0.1f;

        private void Start()
        {
            _linePos = transform.position;
            _linePos.y = LineYPos;
        }

        public Vector3 GetLinePos() => _linePos;
        public Team GetTeam() => team;

        public void IncreasePathsCount() => availablePathsCount++;
        public void DecreasePathsCount() => availablePathsCount--;
    }
}
