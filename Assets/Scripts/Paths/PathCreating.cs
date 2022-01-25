using System.Collections.Generic;
using Buildings;
using UnityEngine;

namespace Paths
{
    public class PathCreating : MonoBehaviour
    {
        [SerializeField] private List<Path> createdPaths;
        [SerializeField] private Path pathPrefab;

        public void CreatePath(Building start, Building end, Team team)
        {
            if (GetPathByPoints(start, end) != null) return;

            if (GetReversePath(start, end) != null)
            {
                if (start.GetTeam() == end.GetTeam())
                {
                    var reverse = GetReversePath(start, end);
                    RemovePath(reverse);
                    FormPath(start, end, team, false);
                }
                else
                {
                    var enemyPath = GetReversePath(start, end);
                    
                    RemovePath(enemyPath);

                    FormPath(start, end, team, true);
                    FormPath(end, start, enemyPath.GetPathTeam(), true);
                }
            }
            else
                FormPath(start, end, team, false);
        }

        public void RemovePath(Path path)
        {
            createdPaths.Remove(path);
            path.GetStartBuilding().RemovePath(path);
        }

        public void CreateLineAfterBattle(Path removedPath)
        {
            var enemyPath = GetReversePath(removedPath.GetStartBuilding(), removedPath.GetEndBuilding());
            if (enemyPath == null)
            {
                Debug.Log("enemy is null");
            }
            else
            {
                RemovePath(removedPath);
                RemovePath(enemyPath);

                CreatePath(enemyPath.GetStartBuilding(), enemyPath.GetEndBuilding(), enemyPath.GetPathTeam());
            }
        }

        public List<Path> GetPathsList() => createdPaths;

        public Path GetPathByPoints(Building start, Building end) =>
            createdPaths.Find(path => path.IfPathsEqual(start, end));

        private Path GetReversePath(Building start, Building end)=>
            createdPaths.Find(path => path.IfPathsReverse(start, end));

        private void FormPath(Building start, Building end, Team team, bool isBattlePath)
        {
            var newPath = Instantiate(pathPrefab, start.transform.position, Quaternion.identity, transform);
            newPath.SetPath(start, end, team, isBattlePath);
            createdPaths.Add(newPath);

            if(isBattlePath)
                newPath.gameObject.name = team +" vs " + end.GetTeam() + " battle path";
            else
                newPath.gameObject.name = team + " path";
            
            start.GetComponent<BuildingPathIndicating>().IncreasePathsCount();
            start.AttachPath(newPath);
        }
    }
}
