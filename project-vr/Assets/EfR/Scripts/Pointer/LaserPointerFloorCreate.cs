using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//ToDo:Root_Commonに置く
public class LaserPointerFloorCreate : LaserPointerBase
{
    GimmickFloor currentControlFloor = null;
    PointerHitScreen preHitScreen = null;

    protected override void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
        SetPosition(hit.point);
        if (!isLocalPlayer) return;
        PointerHitScreen hitScreen = hit.collider.GetComponent<PointerHitScreen>();

        if (!hitScreen || (preHitScreen && hitScreen != preHitScreen)||!CanCreateFloor(hitScreen) )
        {
            TerminateFloor();
            return;
        }
        preHitScreen = hitScreen;
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            CreateFloor(hitScreen.GetFloorForm);
        }
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Z))
        {
            FollowPointerFloor(hit.point, hit.normal);
        }
        else
        {
            TerminateFloor();
        }
    }
    bool CanCreateFloor(PointerHitScreen hitScreen)
    {
        if (PlayerManager.Players.Length <= (int)hitScreen.canCreatePlayerNumber)
        {
            return false;
        }
        return PlayerManager.LocalPlayer == PlayerManager.Players[(int)hitScreen.canCreatePlayerNumber];
    }
    protected override void NoHitAction(Vector3 origin, Vector3 direction)
    {
        base.NoHitAction(origin, direction);
        TerminateFloor();
    }
    void FollowPointerFloor(Vector3 pos, Vector3 normal)
    {
        if (!currentControlFloor) return;
        //currentControlFloor.transform.position = Vector3.Lerp(currentControlFloor.transform.position, pos,0.01f);
        currentControlFloor.transform.forward = normal;
        currentControlFloor.GetComponent<Rigidbody>().MovePosition( pos + normal * 0.1f);
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
        GimmickFloorSpawner.GetInstance().ReleaseFloor(currentControlFloor);
        currentControlFloor = null;
    }
}
