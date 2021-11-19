using Line;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private const int LineLayer = 8;
    private LineDrawer _lineDrawer;

    private void Start() => _lineDrawer = FindObjectOfType<LineDrawer>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LineLayer) return;
        _lineDrawer.SetLineError(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != LineLayer) return;
        _lineDrawer.SetLineError(false);
    }
}
