using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOPoint
{
    Transform IOTransform;
    string pointName;
    public Vector3 Position
    {
        get
        {
            if (IOTransform == null)
            {
                IOTransform = ownerLine.transform.Find(pointName);
            }
            return IOTransform.position;
        }
    }
    //自分を持っているライン
    TransmissionLine ownerLine;
    public TransmissionLine OwnerLine
    {
        get { return ownerLine; }
    }
    public bool IsPowerOn
    {
        get
        {
            return ownerLine.IsPowerOn;
        }
        set
        {
            ownerLine.IsPowerOn = value;
        }
    }
    public IOPoint(string pointName,TransmissionLine ownerLine)
    {
        this.pointName = pointName;
        this.ownerLine = ownerLine;
    }
}
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
    bool isPowerOn=false;
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
    IOPoint point1;
    public IOPoint Point1
    {
        get
        {
            if (point1==null)
            {
                point1 = new IOPoint("Point1", this);
            }
            return point1;
        }
    }

    IOPoint point2;
    public IOPoint Point2
    {
        get
        {
            if (point2 == null)
            {
                point2 = new IOPoint("Point2", this);
            }
            return point2;
        }
    }
    public bool havePower;

    private void Start()
    {
        LineLinker.GetInstance().AddLine(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Point1.Position, Point2.Position);   
    }
    private void Update()
    {
        lineRenderer.SetPosition(0, Point1.Position);
        lineRenderer.SetPosition(1, Point2.Position);
    }
}
