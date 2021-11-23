using Line;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private const int LineLayer = 8;
    private LineDrawing _lineDrawing;

    private void Start() => _lineDrawing = FindObjectOfType<LineDrawing>();

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer != LineLayer) return;
        _lineDrawing.SetLineError(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != LineLayer) return;
        _lineDrawing.SetLineError(false);
    }
}
