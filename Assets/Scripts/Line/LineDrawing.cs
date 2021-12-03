using Buildings;
using UnityEngine;

namespace Line
{
    public class LineDrawing : MonoBehaviour
    {
        [SerializeField] private Material errorMaterial;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private bool isError;
        public Vector3 StartPos { get; set; } = Vector3.zero;
        public Vector3 EndPos { get; set; } = Vector3.zero;

        private Building _startBuilding;
        private LineRenderer _lineRenderer;
        private Vector3[] _positions;
        private Material _currentTeamMaterial;

        private void Awake()
        {
            _lineRenderer = GetComponentInChildren<LineRenderer>();

            RemoveLine();
            _lineRenderer.SetPositions(_positions);
        }

        private void FixedUpdate()
        {
            var distance = Vector3.Distance(StartPos, EndPos);
            var direction = (EndPos - StartPos).normalized;

            if(_startBuilding != null &&!_startBuilding.GetIndicatingComponent().IsPathCreationAvailable())
                SetLineError(true);
            else
            {
                if (Physics.Raycast(StartPos, direction, out var hit, distance, layerMask) && hit.collider != null
                    && hit.collider.gameObject.GetInstanceID() != _startBuilding.gameObject.GetInstanceID())
                {
                    if (hit.transform.GetComponent<Building>() != null)
                        SetLineError(EndPos != hit.transform.GetComponent<Building>().GetLinePos());
                    else
                        SetLineError(true);
                }   
                else
                    SetLineError(false);
            }

        }

        public void RemoveLine()
        {
            StartPos = Vector3.zero;
            EndPos = Vector3.zero;

            _lineRenderer.positionCount = 0;
            _positions = new[] { StartPos, EndPos};
        }

        public void SetLinePos()
        {
            _lineRenderer.positionCount = 2;
            SetBasicLine();
            _lineRenderer.SetPositions(_positions);
        }

        public bool IsLineError() => isError;

        public void SetLineTeamMaterial(Material material)
        {
            _lineRenderer.material = material;
            _currentTeamMaterial = material;
        }
        
        private void SetLineError(bool value)
        {
            _lineRenderer.material = value ? errorMaterial : _currentTeamMaterial;
            isError = value;
        }

        public void SetStartBuilding(Building startBuilding) => _startBuilding = startBuilding;

        private void SetBasicLine() => _positions = new[] { StartPos, EndPos};
    }
}
