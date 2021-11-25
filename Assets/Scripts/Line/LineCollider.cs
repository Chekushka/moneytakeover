using UnityEngine;

namespace Line
{
    public class LineCollider : MonoBehaviour
    {
        private CapsuleCollider _collider;

        private void Awake() => _collider = GetComponentInChildren<CapsuleCollider>();

        public void CreateLineCollider(Vector3 startPos, Vector3 endPos, float width)
        {
            _collider.enabled = true;
            _collider.radius = width / 2;
            _collider.direction = 2;
            _collider.center = Vector3.zero;

            _collider.transform.position = startPos + (endPos - startPos) / 2;
            _collider.transform.LookAt(endPos);
            _collider.height = Vector3.Distance(startPos, endPos);
        }
        
    }
}
