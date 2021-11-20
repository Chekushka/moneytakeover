using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;

namespace Path
{
    public class PathCreating : MonoBehaviour
    {
        [SerializeField] private List<Path> createdPaths;
        [SerializeField] private Path pathPrefab;
        [SerializeField] private Transform pathsParent;
    
        public void CreatePath(Building start, Building end, Team team)
        {
            if (IfPathExist(start, end)) return;

            if (GetReversePath(start, end) != null)
            {
                if (start.GetTeam() == end.GetTeam())
                {
                    var reverse = GetReversePath(start, end);
                    RemovePath(reverse);
                    Destroy(reverse.gameObject);
                    FormPath(start, end, team);
                }
                else
                {
                    //Logic for conquest path;
                }
            }
            else
                FormPath(start, end, team);
        }

        public void RemovePath(Path path)
        {
            path.GetStartBuilding().DecreasePathsCount();
            var pathToRemove = createdPaths.Find(x => x.GetInstanceID() == path.GetInstanceID());
            createdPaths.Remove(pathToRemove);
        }

        private void FormPath(Building start, Building end, Team team)
        {
            var newPath = Instantiate(pathPrefab, start.transform.position, Quaternion.identity, pathsParent);
            newPath.SetPath(start, end, team);
            createdPaths.Add(newPath);
            newPath.gameObject.name = team + " path";
            
            start.IncreasePathsCount();
        }

        private bool IfPathExist(Building start, Building end)
        {
            var isExist = false;
            foreach (var path in createdPaths.Where(path => path.IfPathsEqual(start, end)))
                isExist = true;

            return isExist;
        }

        private Path GetReversePath(Building start, Building end) =>
            createdPaths.Find(path => path.IfPathsReverse(start, end));
       
    }
}
