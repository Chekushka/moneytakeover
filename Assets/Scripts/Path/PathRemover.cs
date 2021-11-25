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
            var path = other.transform.parent.GetComponent<Path>();
            
            if(path.IsPathInBattle())
                _pathCreating.CreateLineAfterBattle(path);
            else
                _pathCreating.RemovePath(path);
        }
    }
}
