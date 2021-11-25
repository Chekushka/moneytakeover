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
            if (GetPathByPoints(start, end) != null) return;

            if (GetReversePath(start, end) != null)
            {
                if (start.GetTeam() == end.GetTeam())
                {
                    var reverse = GetReversePath(start, end);
                    RemovePath(reverse);
                    Destroy(reverse.gameObject);
                    FormPath(start, end, team, false);
                }
                else
                {
                    var enemyPath = GetReversePath(start, end);
                    
                    RemovePath(enemyPath);
                    Destroy(enemyPath.gameObject);
                    
                    FormPath(start, end, team, true);
                    FormPath(end, start, enemyPath.GetPathTeam(), true);
                }
            }
            else
                FormPath(start, end, team, false);
            
            end.UpdateUnitSpawnPower();
        }

        public void RemovePath(Path path)
        {
            path.GetStartBuilding().RemovePath(path);
            var pathToRemove = createdPaths.Find(x => x.GetInstanceID() == path.GetInstanceID());
            createdPaths.Remove(pathToRemove);
            path.GetEndBuilding().UpdateUnitSpawnPower();
        }

        public void CreateLineAfterBattle(Path removedPath)
        {
            var enemyPath = GetReversePath(removedPath.GetStartBuilding(), removedPath.GetEndBuilding());

            RemovePath(removedPath);
            RemovePath(enemyPath);
            
            CreatePath(enemyPath.GetStartBuilding(), enemyPath.GetEndBuilding(), enemyPath.GetPathTeam());
        }

        public List<Path> GetPathsList() => createdPaths;

        public Path GetPathByPoints(Building start, Building end) =>
            createdPaths.Find(path => path.IfPathsEqual(start, end));

        private Path GetReversePath(Building start, Building end) =>
            createdPaths.Find(path => path.IfPathsReverse(start, end));

        private void FormPath(Building start, Building end, Team team, bool isBattlePath)
        {
            var newPath = Instantiate(pathPrefab, start.transform.position, Quaternion.identity, pathsParent);
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
