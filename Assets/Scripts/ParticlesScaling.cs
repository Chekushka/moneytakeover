using System;
using UnityEngine;

public class ParticlesScaling : MonoBehaviour
{
    private void Update()
    {
        var scale = transform.localScale;
        if (!(scale.x > 0.1f)) return;
        scale -= Vector3.one * 0.01f;
        transform.localScale = scale;
    }
}
