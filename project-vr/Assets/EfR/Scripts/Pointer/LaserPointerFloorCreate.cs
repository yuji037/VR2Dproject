using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LaserPointerFloorCreate : LaserPointerBase
{
    protected override void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
        SetPosition(hit.point);
        if (hit.collider.gameObject.tag != "LaserPointerFloorCreate") return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBox(hit.point);
        }
    }

    void CreateBox(Vector3 pos)
    {
        var obj = StageObjectPool.GetInstance().GetPoolStageObject();
        obj.transform.position = pos;
    }
}
