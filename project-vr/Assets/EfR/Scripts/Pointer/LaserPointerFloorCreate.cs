using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//ToDo:Root_Commonに置く
public class LaserPointerFloorCreate : LaserPointerBase
{
    GimmickFloor controllingFloor = null;

    [SerializeField]
    FloorPredictionActiveController floorPredictionActiveController;

    bool isAlwaysPressingTrigger=false;
    private void Awake()
    {
        DebugTools.RegisterDebugAction(
                KeyCode.G,
            () => {
                isFixPointer = !isFixPointer;
                isAlwaysPressingTrigger = !isAlwaysPressingTrigger;
            },
            "常に床生成のトリガーを押した状態にする"
            );

    }

    protected override void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
        SetLineRenderPosition(hit.point);
        if (!isLocalPlayer) return;
        PointerHitScreen hitScreen = hit.collider.GetComponent<PointerHitScreen>();
        if (!(hitScreen &&
            CanCreateFloor(hitScreen)))
        {
            DeleteFloor();
            return;
        }

        if (IsPressDownTrigger())
        {
            CreateFloor(hitScreen.GetFloorForm, hitScreen);
        }

        if (controllingFloor)
        {
            ControlFloor(hit.point, hit.normal, hitScreen);
        }
        else
        {
            floorPredictionActiveController.SetView(hit.point +hit.normal* 0.1f, hitScreen);
        }

    }
    void ControlFloor(Vector3 hitPoint, Vector3 hitNoraml, PointerHitScreen hitScreen)
    {
        bool floorFormEqual = controllingFloor.floorForm == hitScreen.GetFloorForm;
        if (IsPressingTrigger()&&floorFormEqual)
        {
            floorPredictionActiveController.AllInactive();
            FollowPointerFloor(hitPoint, hitNoraml);
        }
        else
        {
            DeleteFloor();
        }

    }
    bool IsPressDownTrigger()
    {
        return Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z) || OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
    }
    bool IsPressingTrigger()
    {
        return Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Z) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger) ||isAlwaysPressingTrigger;
    }
    bool CanCreateFloor(PointerHitScreen hitScreen)
    {
        return PlayerManager.GetPlayerNumber()== (int)hitScreen.canCreatePlayerNumber;
    }
    protected override void NoHitAction(Vector3 origin, Vector3 direction)
    {
        base.NoHitAction(origin, direction);
        floorPredictionActiveController.AllInactive();
        DeleteFloor();
    }
    void FollowPointerFloor(Vector3 pos, Vector3 normal)
    {
        if (!controllingFloor) return;
        controllingFloor.GetComponent<Rigidbody>().MovePosition(pos + normal * 0.1f);
    }
    void CreateFloor(FloorForm floorForm, PointerHitScreen pointerHitScreen)
    {
        DeleteFloor();
        GimmickFloorSpawner.GetInstance().GetFloorObject(floorForm,
              (x) =>
              {
                  controllingFloor = x;
              }
          );
    }


    void DeleteFloor()
    {
        if (!controllingFloor) return;
        GimmickFloorSpawner.GetInstance().ReleaseFloor(controllingFloor);
        controllingFloor = null;
    }
}
