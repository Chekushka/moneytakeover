using UnityEngine;

namespace Path
{
    public class PathRemover : MonoBehaviour
    {
        private const int PathLayer = 10;
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.layer != PathLayer) return;
            Destroy(other.transform.parent.gameObject);
        }
    }
}
