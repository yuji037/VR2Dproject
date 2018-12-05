using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//ToDo:Root_Commonに置く
public class LaserPointerFloorCreate : LaserPointerBase
{
    GimmickFloor currentControlFloor = null;
    PointerHitScreen preHitScreen=null;
    
    protected override void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
        SetPosition(hit.point);
        if (!isLocalPlayer) return;
        PointerHitScreen hitScreen = hit.collider.GetComponent<PointerHitScreen>();
        if (!hitScreen) return;
        if (!hitScreen || (preHitScreen && hitScreen != preHitScreen))
        {
            TerminateFloor();
            return;
        }
        preHitScreen = hitScreen;
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z))
        {
            CreateFloor(hitScreen.GetFloorForm);
        }
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Z))
        {
            FollowPointerFloor(hit.point);
        }
        else
        {
            TerminateFloor();
        }
    }

    protected override void NoHitAction(Vector3 origin, Vector3 direction)
    {
        base.NoHitAction(origin,direction);
        TerminateFloor();
    }
    void FollowPointerFloor(Vector3 pos)
    {
        if (!currentControlFloor) return;
        //currentControlFloor.transform.position = Vector3.Lerp(currentControlFloor.transform.position, pos,0.01f);
        currentControlFloor.transform.position = pos+new Vector3(0,0,-0.1f);
    }
    void CreateFloor(FloorForm floorForm)
    {
        TerminateFloor();
        GimmickFloorSpawner.GetInstance().GetFloorObject(floorForm,
              (x) =>
              {
                  currentControlFloor = x;
              }
          );
    }

    void TerminateFloor()
    {
        preHitScreen = null;
        if (!currentControlFloor) return;
        NetworkServer.Destroy(currentControlFloor.gameObject);
        currentControlFloor = null;
    }
}
