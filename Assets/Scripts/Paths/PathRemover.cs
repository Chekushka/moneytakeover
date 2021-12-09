using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Paths
{
    public class PathRemover : MonoBehaviour
    {
        private PathCreating _pathCreating;
        private const int PathLayer = 10;

        private void OnEnable()
        {
            _pathCreating = FindObjectOfType<PathCreating>();
        }

        private void OnDisable()
        {
            _pathCreating = null;
        }

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
