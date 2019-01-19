using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public enum FloorForm
{
    Normal,
    LongX,
    LongY,
}
public class GimmickFloor : GimmickBase
{
    public FloorForm floorForm;
    BoxColliderInfo fBox;
    Vector3 defaultBoxSize;
    Vector3 defaultBoxCenter;
    BoxCollider boxCollider;
    private new void Awake()
    {
        base.Awake();
        fBox = GetComponent<BoxColliderInfo>();
        boxCollider = GetComponent<BoxCollider>();
        defaultBoxSize = boxCollider.size;
        defaultBoxCenter = boxCollider.center;
    }
    [ClientRpc]
    public void RpcInit(NetworkIdentity playerNetID)
    {
        if (PlayerManager.LocalPlayer.GetComponent<NetworkIdentity>() == playerNetID)
        {
            GimmickFloorSpawner.GetInstance().RegisterFloor(this);
        }
    }

    public void CorrectCollider(PointerHitScreen pointerHitScreen)
    {
        Vector3 boxSize = defaultBoxSize;
        Vector3 boxPos = defaultBoxCenter;
        var sBox = pointerHitScreen.GetComponent<BoxColliderInfo>();
        var pivot = toYzero(pointerHitScreen.transform.position);
        var rot = pointerHitScreen.transform.rotation;

        var rSub = (
                Vector3.Distance(toYzero(fBox.RightPoint), pivot) -
                Vector3.Distance(toYzero(sBox.RightPoint), pivot)
                ) / transform.lossyScale.x;
        if (rSub > 0)
        {
            boxSize.x = defaultBoxSize.x - rSub;
            boxPos = new Vector3(-rSub / 2, boxPos.y, boxPos.z);
        }

        var lSub = (
                Vector3.Distance(toYzero(fBox.LeftPoint), pivot) -
                Vector3.Distance(toYzero(sBox.LeftPoint), pivot)
                ) / transform.lossyScale.x;
        if (lSub > 0)
        {
            boxSize.x = defaultBoxSize.x - lSub;
            boxPos = new Vector3(lSub / 2, boxPos.y, boxPos.z);
        }

        pivot = toXzero(pointerHitScreen.transform.position);

        var tSub = (
               Vector3.Distance(toXzero(fBox.TopPoint), pivot) -
               Vector3.Distance(toXzero(sBox.TopPoint), pivot)
               ) / transform.lossyScale.y;
        if (tSub > 0)
        {
            boxSize.y = defaultBoxSize.y - tSub;
            boxPos = new Vector3(boxPos.x, -tSub / 2, boxPos.z);
        }

        var bSub = (
               Vector3.Distance(toXzero(fBox.BottomPoint), pivot) -
               Vector3.Distance(toXzero(sBox.BottomPoint), pivot)
               ) / transform.lossyScale.y;
        if (bSub > 0)
        {
            boxSize.y = defaultBoxSize.y - bSub;
            boxPos = new Vector3(boxPos.x, bSub / 2, boxPos.z);
        }

        boxCollider.size = boxSize;
        boxCollider.center = boxPos;
    }
    Vector3 toYzero(Vector3 vector)
    {
        vector.y = 0;
        return vector;
    }
    Vector3 toXzero(Vector3 vector)
    {
        vector.x = 0;
        return vector;
    }
}
