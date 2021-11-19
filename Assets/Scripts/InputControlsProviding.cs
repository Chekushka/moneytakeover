using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using Line;
using Path;
using UnityEngine;

public class InputControlsProviding : MonoBehaviour
{
    [SerializeField] private LineDrawer lineDrawer;
    [SerializeField] private GameObject lineRemover;
    [SerializeField] private LayerMask inputLayerMask;
    [SerializeField] private Team playerTeam;
    [SerializeField] private PathCreating pathCreating;

    private Building _startBuilding;
    private Building _endBuilding;
    private bool _isRemoverActive;

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
            lineDrawer.EndPos = new Vector3(hit.point.x, 0.1f, hit.point.z);
            lineDrawer.SetLinePos();
        }
        
        if (touch.phase == TouchPhase.Moved)
        {
            if (hit.collider.gameObject.layer == BuildingLayer && !_isRemoverActive)
            {
                if (hit.transform.gameObject.GetInstanceID() != _startBuilding.gameObject.GetInstanceID() 
                    && _startBuilding != null)
                {
                    _endBuilding = hit.transform.GetComponent<Building>();
                    lineDrawer.EndPos = _endBuilding.GetLinePos();
                }
            }

            if (_startBuilding == null)
            {
                lineRemover.SetActive(true);
                lineRemover.transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
                _isRemoverActive = true;
            }
        }

        if (hit.collider.gameObject.layer != BuildingLayer &&
            touch.phase == TouchPhase.Moved && _startBuilding != null)
            _endBuilding = null;

        if (touch.phase == TouchPhase.Ended)
        {
            lineDrawer.RemoveLine();
            if (_startBuilding != null && _endBuilding != null && !lineDrawer.isError)
                pathCreating.CreatePath(_startBuilding, _endBuilding, _startBuilding.GetTeam());
            _startBuilding = null;
            _endBuilding = null;
            
            lineRemover.SetActive(false);
            _isRemoverActive = false;
        }
    }
}
