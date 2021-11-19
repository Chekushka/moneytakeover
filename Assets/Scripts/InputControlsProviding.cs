using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

public class InputControlsProviding : MonoBehaviour
{
    [SerializeField] private LineDrawer lineDrawer;
    [SerializeField] private LayerMask inputLayerMask;
    [SerializeField] private Team playerTeam;
    [SerializeField] private PathCreating pathCreating;

    private Building _startBuilding;
    private Building _endBuilding;

    private const int BuildingLayer = 6;

    private void Start()
    {
        lineDrawer.SetLineTeamMaterial(TeamColors.GetInstance().GetLineMaterial(playerTeam));
    }

    private void Update()
    {
        if (Input.touchCount <= 0) return;
        var touch = Input.GetTouch(0);
       
        var ray = Camera.main.ScreenPointToRay(touch.position);
   
        if (!Physics.Raycast(ray, out var hit, inputLayerMask)) return;
        if (hit.collider == null) return;

        if (touch.phase == TouchPhase.Began && hit.collider.gameObject.layer == BuildingLayer)
        {
            _startBuilding = hit.transform.GetComponent<Building>();
            if (_startBuilding.GetTeam() == playerTeam)
            {
                lineDrawer.StartPos = _startBuilding.GetLinePos();
                lineDrawer.SetLinePos();
            }
        }
        
        if (lineDrawer.StartPos != Vector3.zero && _endBuilding == null)
        {
            lineDrawer.EndPos = hit.point;
            lineDrawer.SetLinePos();
        }

        if (touch.phase == TouchPhase.Moved && hit.collider.gameObject.layer == BuildingLayer)
        {
            if (hit.transform.gameObject.GetInstanceID() != _startBuilding.gameObject.GetInstanceID() 
                && _startBuilding != null)
            {
                _endBuilding = hit.transform.GetComponent<Building>();
                lineDrawer.EndPos = _endBuilding.GetLinePos();
            }
        }

        if (hit.collider.gameObject.layer != BuildingLayer &&
            touch.phase == TouchPhase.Moved && _startBuilding != null)
        {
            _endBuilding = null;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            lineDrawer.RemoveLine();
            if (_startBuilding != null && _endBuilding != null)
                pathCreating.CreatePath(_startBuilding, _endBuilding, _startBuilding.GetTeam());
            _startBuilding = null;
            _endBuilding = null;
        }
    }
}
