using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FloorColliderCorrecter : MonoBehaviour
{
    bool wasScreenBind = false;
    BoxInfo fBox;
    Vector3 defaultBoxSize;
    Vector3 defaultBoxCenter;
    BoxCollider boxCollider;
    PointerHitScreen stickScreen;

    private void Awake()
    {
        fBox = GetComponent<BoxInfo>();
        boxCollider = GetComponent<BoxCollider>();
        defaultBoxSize = boxCollider.size;
        defaultBoxCenter = boxCollider.center;
    }

    void BindScreen(PointerHitScreen screen)
    {
        wasScreenBind = true;
        this.stickScreen = screen;
        transform.rotation = stickScreen.transform.rotation;
    }
    PointerHitScreen FindNearScreen()
    {
        var colliders=Physics.OverlapBox(transform.position,defaultBoxSize,transform.rotation);
        foreach (var i in colliders)
        {
            var screen = i.GetComponent<PointerHitScreen>();
            if (screen)
            {
                return screen;
            }
        }
        return null;
    }
    private void Update()
    {

        if (wasScreenBind)
        {
            CorrectCollider();
        }
        else
        {
            var screen=FindNearScreen();
            if (screen)
            {
                BindScreen(screen);
            }
        }
    }
    void CorrectCollider()
    {
        Vector3 boxSize = defaultBoxSize;
        Vector3 boxPos = defaultBoxCenter;
        //screenBox
        var sBox = stickScreen.GetComponent<BoxInfo>();
        var pivot = stickScreen.transform.position;

        var srDis = (sBox.RightPoint - pivot).magnitude;
        var srVec = (sBox.RightPoint - pivot).normalized;
        var frDis = Vector3.Dot(srVec, (fBox.RightPoint - pivot));
        var rSub = (frDis - srDis) / transform.lossyScale.x;
        if (rSub > 0)
        {
            boxSize.x = defaultBoxSize.x - rSub;
            boxPos = new Vector3(-rSub / 2, boxPos.y, boxPos.z);
        }


        var slDis = (sBox.LeftPoint - pivot).magnitude;
        var slVec = (sBox.LeftPoint - pivot).normalized;
        var flDis = Vector3.Dot(slVec, (fBox.LeftPoint - pivot));
        var lSub = (flDis - slDis) / transform.lossyScale.x;
        if (lSub > 0)
        {
            boxSize.x = defaultBoxSize.x - lSub;
            boxPos = new Vector3(lSub / 2, boxPos.y, boxPos.z);
        }

        var stDis = (sBox.TopPoint - pivot).magnitude;
        var stVec = (sBox.TopPoint - pivot).normalized;
        var ftDis = Vector3.Dot(stVec, (fBox.TopPoint - pivot));
        var tSub = (ftDis - stDis) / transform.lossyScale.y;
        if (tSub > 0)
        {
            boxSize.y = defaultBoxSize.y - tSub;
            boxPos = new Vector3(boxPos.x, -tSub / 2, boxPos.z);
        }

        var sbDis = (sBox.BottomPoint - pivot).magnitude;
        var sbVec = (sBox.BottomPoint - pivot).normalized;
        var fbDis = Vector3.Dot(sbVec, (fBox.BottomPoint - pivot));
        var bSub = (fbDis - sbDis) / transform.lossyScale.y;
        if (bSub > 0)
        {
            boxSize.y = defaultBoxSize.y - bSub;
            boxPos = new Vector3(boxPos.x, bSub / 2, boxPos.z);
        }

        if (boxSize.x > 0 && boxSize.y > 0 && boxSize.z > 0) boxCollider.size = boxSize;
        boxCollider.center = boxPos;
    }
}
