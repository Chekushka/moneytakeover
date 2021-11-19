using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    public Vector3 StartPos { get; set; } = Vector3.zero;
    public Vector3 EndPos { get; set; } = Vector3.zero;
    
    private LineRenderer _lineRenderer;
    private Vector3[] _positions;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        
        RemoveLine();
        _lineRenderer.SetPositions(_positions);
    }

    public void RemoveLine()
    {
        StartPos = Vector3.zero;
        EndPos = Vector3.zero;

        _lineRenderer.positionCount = 0;
        _positions = new[] { StartPos, EndPos};
    }

    public void SetLinePos()
    {
        _lineRenderer.positionCount = 2;
        SetBasicLine();
        _lineRenderer.SetPositions(_positions);
    }

    public void SetLineTeamMaterial(Material material) => _lineRenderer.material = material;

    private void SetBasicLine() => _positions = new[] { StartPos, EndPos};
}
