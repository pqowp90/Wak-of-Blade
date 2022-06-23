using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererSet : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private void Start() {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.alignment = LineAlignment.TransformZ;
    }
}
