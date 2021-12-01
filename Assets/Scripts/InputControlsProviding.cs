using Buildings;
using Line;
using Paths;
using UnityEngine;

public class InputControlsProviding : MonoBehaviour
{
    [SerializeField] private LineDrawing lineDrawing;
    [SerializeField] private GameObject lineRemover;
    [SerializeField] private LayerMask inputLayerMask;
    [SerializeField] private PathCreating pathCreating;

    private Team _playerTeam;
    private Building _startBuilding;
    private Building _endBuilding;
    private bool _isRemoverActive;

    private const int BuildingLayer = 6;

    private void Start()
    {
        _playerTeam = TeamAssignment.GetInstance().GetPlayerTeam();
        lineDrawing.SetLineTeamMaterial(TeamColors.GetInstance().GetLineMaterial(_playerTeam));
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
            if (_startBuilding.GetTeam() == _playerTeam)
            {
                if (_startBuilding.GetTeam() == _playerTeam)
                {
                    lineDrawing.StartPos = _startBuilding.GetLinePos();
                    lineDrawing.SetLinePos();
                    lineDrawing.SetStartBuilding(_startBuilding);
                }
            }
            else
                _startBuilding = null;
        }
        
        if (lineDrawing.StartPos != Vector3.zero && _endBuilding == null)
        {
            lineDrawing.EndPos = new Vector3(hit.point.x, 0.1f, hit.point.z);
            lineDrawing.SetLinePos();
        }
        
        if (touch.phase == TouchPhase.Moved)
        {
            if (hit.collider.gameObject.layer == BuildingLayer && !_isRemoverActive && _startBuilding != null)
            {
                if (hit.transform.gameObject.GetInstanceID() != _startBuilding.gameObject.GetInstanceID())
                {
                    _endBuilding = hit.transform.GetComponent<Building>();
                    lineDrawing.EndPos = _endBuilding.GetLinePos();
                    lineDrawing.SetLinePos();
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
        {
            _endBuilding = null;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            lineDrawing.RemoveLine();
            if (_startBuilding != null && _endBuilding != null && !lineDrawing.isError &&
                _startBuilding.GetComponent<BuildingPathIndicating>().IsPathCreationAvailable())
                pathCreating.CreatePath(_startBuilding, _endBuilding, _startBuilding.GetTeam());
            _startBuilding = null;
            _endBuilding = null;
            
            lineRemover.SetActive(false);
            _isRemoverActive = false;
        }
    }
}
