using System;
using UnityEngine;

namespace Path
{
    public class PathRemover : MonoBehaviour
    {
        private PathCreating _pathCreating;
        private const int PathLayer = 10;

        private void Start() => _pathCreating = FindObjectOfType<PathCreating>();

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.layer != PathLayer) return;
            _pathCreating.RemovePath(other.transform.parent.GetComponent<Path>());
            Destroy(other.transform.parent.gameObject);
        }
    }
}
