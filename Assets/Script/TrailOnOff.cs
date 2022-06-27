using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailOnOff : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }
    void Update()
    {
        trailRenderer.enabled = trailRenderer.time>=0.01f;
    }
}
