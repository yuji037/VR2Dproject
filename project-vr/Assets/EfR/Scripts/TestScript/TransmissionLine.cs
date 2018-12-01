using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class TransmissionLine : MonoBehaviour
{
    LineRenderer lr;
    LineRenderer lineRenderer
    {
        get
        {
            if (!lr)
            {
                lr = GetComponent<LineRenderer>();
            }
            return lr;
        }
    }
    bool isPowerOn = false;
    public bool IsPowerOn
    {
        get { return isPowerOn; }
        set
        {
            isPowerOn = value;
            if (isPowerOn)
            {
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
            }
            else
            {
                lineRenderer.startColor = Color.white;
                lineRenderer.endColor = Color.white;
            }
        }
    }
    [SerializeField]
    List<IOPoint> ioPoints = new List<IOPoint>();
    public List<IOPoint> IOPoints
    {
        get
        {
            return ioPoints;
        }
    }
#if UNITY_EDITOR
    public void AddPoint()
    {
        var obj = new GameObject("Point");
        var p =Undo.AddComponent<IOPoint>(obj);
        p.transform.parent = transform;
        if (IOPoints.Count > 0)
        {
            p.transform.position = IOPoints[IOPoints.Count-1].transform.position;
        }
        else
        {
            p.transform.localPosition = Vector3.zero;
        }
        ioPoints.Add(p);
        Undo.RecordObject(this, "AddPOint");
        Undo.RegisterCreatedObjectUndo(obj,"CreateNewPoint");
        
    }
#endif
    public bool havePower;

    private void Start()
    {
        LineLinker.GetInstance().AddLine(this);
    }
    private void OnDrawGizmos()
    {
        RemoveNullPoint();
        Gizmos.color = Color.red;
        IOPoint prePoint = null;
        foreach (var p in ioPoints)
        {
            if (prePoint)
            {
                Gizmos.DrawLine(prePoint.Position, p.Position);
            }
            prePoint = p;
        }
    }
    private void Update()
    {
        RemoveNullPoint();
        if (ioPoints.Count != lineRenderer.positionCount)
        {
            lineRenderer.positionCount = ioPoints.Count;
        }
        int num = 0;
        foreach (var p in ioPoints)
        {
            lineRenderer.SetPosition(num, p.Position);
            num++;
        }
    }

    void RemoveNullPoint()
    {
        ioPoints.RemoveAll(x=>x==null);
    }
}
