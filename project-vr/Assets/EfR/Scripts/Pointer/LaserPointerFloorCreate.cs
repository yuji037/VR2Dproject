using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LaserPointerFloorCreate : LaserPointerBase
{
    protected override void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
        SetPosition(hit.point);
        if (hit.collider.gameObject.tag != "Screen")
        {
            TerminateBox();
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Z))
        {
            CreateBox(hit.point);
        }
        else { TerminateBox(); }
    }

    protected override void NoHitAction(Vector3 origin, Vector3 direction)
    {
        TerminateBox();
    }

    void CreateBox(Vector3 pos)
    {
        var obj = StageObjectPool.GetInstance().GetPoolStageObject();
        obj.SetActive(true);
        obj.transform.position = pos;
    }

    private void TerminateBox()
    {
        var obj = StageObjectPool.GetInstance().GetPoolStageObject();
        obj.SetActive(false);
    }
}
