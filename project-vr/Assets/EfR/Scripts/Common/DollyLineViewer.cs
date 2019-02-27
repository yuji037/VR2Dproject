using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(LineRenderer)), RequireComponent(typeof(CinemachineSmoothPath))]
public class DollyLineViewer : MonoBehaviour
{
    LineRenderer lineRenderer;
    CinemachineSmoothPath dollyPath;

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        dollyPath = GetComponent<CinemachineSmoothPath>();

        var waypoints = dollyPath.m_Waypoints;
        lineRenderer.positionCount = waypoints.Length+((dollyPath.Looped)?1:0);
        int num = 0;
        foreach (var way in waypoints)
        {
            lineRenderer.SetPosition(num, transform.position+transform.rotation* way.position);
            num++;
        }
        if (dollyPath.Looped)
        {
            lineRenderer.SetPosition(num, transform.position + transform.rotation * waypoints[0].position);
        }
    }

}
