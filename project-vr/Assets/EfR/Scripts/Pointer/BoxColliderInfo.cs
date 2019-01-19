using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderInfo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        float correct = 0.1f;
        Gizmos.color = Color.black;



        var upper = TopSide;
        var right = RightSide;
        var left = LeftSide;
        var lower = BottomSide;
        Gizmos.DrawLine(new Vector3(right, upper, transform.position.z - correct), new Vector3(left, upper, transform.position.z - correct));

        Gizmos.DrawLine(new Vector3(right, lower, transform.position.z - correct), new Vector3(left, lower, transform.position.z - correct));

        Gizmos.DrawLine(new Vector3(right, upper, transform.position.z - correct), new Vector3(right, lower, transform.position.z - correct));

        Gizmos.DrawLine(new Vector3(left, lower, transform.position.z - correct), new Vector3(left, upper, transform.position.z - correct));


    }
    public float HalfWidth
    {
        get { return transform.lossyScale.x * 0.5f; }
    }
    public float HalfHeight
    {
        get { return transform.lossyScale.y * 0.5f; }
    }
    public float BottomSide
    {
        get
        {
            return transform.position.y - transform.lossyScale.y / 2;
        }
    }
    public float TopSide
    {
        get
        {
            return transform.position.y + transform.lossyScale.y / 2;
        }
    }
    public float RightSide
    {
        get
        {
            return transform.position.x + transform.lossyScale.x / 2;
        }
    }
    public float LeftSide
    {
        get
        {
            return transform.position.x - transform.lossyScale.x / 2;
        }
    }
    public Vector3 RightPoint
    {
        get
        {
            return transform.position + transform.rotation*new Vector3(HalfWidth,0,0);
        }
    }

    public Vector3 LeftPoint
    {
        get
        {
            return transform.position + transform.rotation * new Vector3(-HalfWidth,0, 0);
        }
    }

    public Vector3 TopPoint
    {
        get
        {
            return transform.position + transform.rotation * new Vector3(0, HalfHeight, 0);
        }
    }

    public Vector3 BottomPoint
    {
        get
        {
            return transform.position + transform.rotation* new Vector3(0,-HalfHeight,  0);
        }
    }

    Vector3 RightUp
    {
        get
        {
            return transform.position + new Vector3(HalfWidth, HalfHeight);
        }
    }
    Vector3 LeftUp
    {
        get
        {
            return transform.position + new Vector3(-HalfWidth, HalfHeight);
        }
    }
    Vector3 RightDown
    {
        get
        {
            return transform.position + new Vector3(HalfWidth, -HalfHeight);
        }
    }
    Vector3 LeftDown
    {
        get
        {
            return transform.position + new Vector3(-HalfWidth, -HalfHeight);
        }
    }

}
