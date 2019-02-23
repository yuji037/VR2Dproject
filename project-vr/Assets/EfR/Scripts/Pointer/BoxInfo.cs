using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInfo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (!GetComponent<BoxCollider>()) return;
        Gizmos.color = Color.black;

        //top
        Gizmos.DrawLine(RightUp, LeftUp);

        //bot
        Gizmos.DrawLine(RightDown, LeftDown);

        //right
        Gizmos.DrawLine(RightUp, RightDown);

        //left
        Gizmos.DrawLine(LeftUp, LeftDown);

        Gizmos.DrawLine(transform.position,transform.position+ transform.forward*5);

    }


    public float HalfWidth
    {
        get { return transform.lossyScale.x * 0.5f * (GetComponent<BoxCollider>() ? GetComponent<BoxCollider>().size.x : 1f); }
    }
    public float HalfHeight
    {
        get { return transform.lossyScale.y * 0.5f * (GetComponent<BoxCollider>() ? GetComponent<BoxCollider>().size.y : 1f); }
    }
    //public float BottomSide
    //{
    //    get
    //    {
    //        return transform.position.y - transform.lossyScale.y / 2 * GetComponent<BoxCollider>().size.y;
    //    }
    //}
    //public float TopSide
    //{
    //    get
    //    {
    //        return transform.position.y + transform.lossyScale.y / 2 * GetComponent<BoxCollider>().size.y;
    //    }
    //}
    //public float RightSide
    //{
    //    get
    //    {
    //        return transform.position.x + transform.lossyScale.x / 2 * GetComponent<BoxCollider>().size.x;
    //    }
    //}
    //public float LeftSide
    //{
    //    get
    //    {
    //        return transform.position.x - transform.lossyScale.x / 2 * GetComponent<BoxCollider>().size.x;
    //    }
    //}
    public Vector3 RightPoint
    {
        get
        {
            return transform.position + transform.rotation * new Vector3(HalfWidth, 0, 0);
        }
    }

    public Vector3 LeftPoint
    {
        get
        {
            return transform.position + transform.rotation * new Vector3(-HalfWidth, 0, 0);
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
            return transform.position + transform.rotation * new Vector3(0, -HalfHeight, 0);
        }
    }

    Vector3 RightUp
    {
        get
        {
            return transform.position + transform.rotation * new Vector3(HalfWidth, HalfHeight);
        }
    }
    Vector3 LeftUp
    {
        get
        {
            return transform.position + transform.rotation * new Vector3(-HalfWidth, HalfHeight);
        }
    }
    Vector3 RightDown
    {
        get
        {
            return transform.position + transform.rotation * new Vector3(HalfWidth, -HalfHeight);
        }
    }
    Vector3 LeftDown
    {
        get
        {
            return transform.position + transform.rotation * new Vector3(-HalfWidth, -HalfHeight);
        }
    }

}
