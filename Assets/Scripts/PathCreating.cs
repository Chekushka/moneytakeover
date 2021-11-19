using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;

public class PathCreating : MonoBehaviour
{
    [SerializeField] private List<Path> createdPaths;
    [SerializeField] private Path pathPrefab;
    [SerializeField] private Transform pathsParent;
    
    public void CreatePath(Building start, Building end, Team team)
    {
        if (IfPathExist(start, end)) return;

        var newPath = Instantiate(pathPrefab, start.transform.position, Quaternion.identity, pathsParent);
        newPath.SetPath(start, end, team);
        createdPaths.Add(newPath);
        newPath.gameObject.name = team + " path";
    }

    private bool IfPathExist(Building start, Building end)
    {
        var isExist = false;
        foreach (var path in createdPaths.Where(path => path.ComparePathPoints(start, end)))
            isExist = true;

        return isExist;
    }
}
