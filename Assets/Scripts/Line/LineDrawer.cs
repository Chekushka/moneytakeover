using UnityEngine;

namespace Line
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineDrawer : MonoBehaviour
    {
        [SerializeField] private Material errorMaterial;
        public Vector3 StartPos { get; set; } = Vector3.zero;
        public Vector3 EndPos { get; set; } = Vector3.zero;
        public bool isError;
    
        private LineRenderer _lineRenderer;
        private Vector3[] _positions;
        private LineCollider _lineCollider;
        private Material _currentTeamMaterial;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineCollider = GetComponentInChildren<LineCollider>();
        
            RemoveLine();
            _lineRenderer.SetPositions(_positions);
        }

        public void RemoveLine()
        {
            StartPos = Vector3.zero;
            EndPos = Vector3.zero;

            _lineRenderer.positionCount = 0;
            _positions = new[] { StartPos, EndPos};
            _lineCollider.enabled = false;
        }

        public void SetLinePos()
        {
            _lineRenderer.positionCount = 2;
            SetBasicLine();
            _lineRenderer.SetPositions(_positions);
            _lineCollider.CreateLineCollider(StartPos, EndPos, _lineRenderer.startWidth, true);
        }

        public void SetLineTeamMaterial(Material material)
        {
            _lineRenderer.material = material;
            _currentTeamMaterial = material;
        }

    
        public void SetLineError(bool value)
        {
            _lineRenderer.material = value ? errorMaterial : _currentTeamMaterial;
            isError = value;
        }

        private void SetBasicLine() => _positions = new[] { StartPos, EndPos};
    }
}
