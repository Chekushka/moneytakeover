using Buildings;
using Line;
using UnityEngine;

namespace Path
{
    [RequireComponent(typeof(LineRenderer))]
    public class Path : MonoBehaviour
    {
        [SerializeField] private float textureScrollSpeed = 1f;
        [SerializeField] private Team team;
        [SerializeField] private bool isBattlePath;
        
        private LineRenderer _line;
        private LineCollider _lineCollider;
        private Building _startBuilding;
        private Building _endBuilding;
        private bool _isPathCreated;

        private void Update()
        {
            if(_isPathCreated)
                _line.materials[1].mainTextureOffset = new Vector2(-textureScrollSpeed * Time.time, 0);
        }

        public void SetPath(Building start, Building end, Team teamToSet,bool isBattle)
        {
            isBattlePath = isBattle;
            _startBuilding = start;
            _endBuilding = end;
            team = teamToSet;
            _line = GetComponent<LineRenderer>();
            _lineCollider = GetComponent<LineCollider>();
        
            FormPathLine();
            if(team == TeamAssignment.GetInstance().GetPlayerTeam())
                _lineCollider.CreateLineCollider(_startBuilding.transform.position,
                    _endBuilding.transform.position, _line.startWidth);
            _line.material = TeamColors.GetInstance().GetLineMaterial(teamToSet);
            _isPathCreated = true;
        }

        public bool IfPathsEqual(Building start, Building end) => start.GetInstanceID() ==
            _startBuilding.GetInstanceID() && end.GetInstanceID() == _endBuilding.GetInstanceID();

        public bool IfPathsReverse(Building start, Building end) => start.GetInstanceID() == 
            _endBuilding.GetInstanceID() && end.GetInstanceID() == _startBuilding.GetInstanceID();

        public Team GetPathTeam() => team;
        public bool IsPathInBattle() => isBattlePath;
        public Vector3 GetPathStartPos() => _startBuilding.GetLinePos();
        public Vector3 GetPathEndPos() => _endBuilding.GetLinePos();
        public Building GetStartBuilding() => _startBuilding;
        public Building GetEndBuilding() => _endBuilding;

        private void FormPathLine()
        {
            var startPos = _startBuilding.GetLinePos();
            var endPos = _endBuilding.GetLinePos();

            if (isBattlePath)
                endPos = new Vector3((startPos.x + endPos.x) / 2, endPos.y, (startPos.z + endPos.z) / 2);
           
            var positions = new[] { startPos, endPos};
            
            _line.positionCount = 2;
            _line.SetPositions(positions);
            transform.rotation = Quaternion.Euler(90,0,0);
        }
    }
}