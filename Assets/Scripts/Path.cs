using System;
using Buildings;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Path : MonoBehaviour
{
    [SerializeField] private float textureScrollSpeed = 1f;
    
    private Team _team;
    private LineRenderer _line;
    private Building _startBuilding;
    private Building _endBuilding;
    private bool _isPathCreated;

    private void Update()
    {
        if(_isPathCreated)
            _line.materials[0].mainTextureOffset = new Vector2(-textureScrollSpeed * Time.time, 0);
    }

    public void SetPath(Building start, Building end, Team team)
    {
        _startBuilding = start;
        _endBuilding = end;
        _team = team;
        _line = GetComponent<LineRenderer>();

        FormPathLine();
        _line.materials[1] = TeamColors.GetInstance().GetLineMaterial(team);
        
        _isPathCreated = true;
    }

    public bool ComparePathPoints(Building start, Building end)
    {
        var isEqual = (start.GetInstanceID() == _startBuilding.GetInstanceID() && 
                       end.GetInstanceID() == _endBuilding.GetInstanceID()) ||
                      (start.GetInstanceID() == _endBuilding.GetInstanceID()
                       && end.GetInstanceID() == _startBuilding.GetInstanceID());
        return isEqual;
    }

    private void FormPathLine()
    {
        var positions = new[] { _startBuilding.GetLinePos(), _endBuilding.GetLinePos()};
        
        _line.positionCount = 2;
        _line.SetPositions(positions);
    }
}