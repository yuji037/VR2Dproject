using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//ToDo:Root_Commonに置く
public class LaserPointerFloorCreate : LaserPointerBase
{
    GimmickFloor controllingFloor = null;

    FloorPredictionActiveController floorPredictionActiveController;

    bool isAlwaysPressingTrigger=false;

    PointerHitScreen currentHitScreen;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        floorPredictionActiveController=FloorPredictionActiveController.GetInstance();
        DebugTools.RegisterDebugAction(
                KeyCode.G,
            () => {
                isFixPointer = !isFixPointer;
                isAlwaysPressingTrigger = !isAlwaysPressingTrigger;
            },
            "常に床生成のトリガーを押した状態にする"
            );

    }

    bool CanCreateFloor(Vector3 hitPos,PointerHitScreen hitScreen)
    {
        var prefab=GimmickFloorSpawner.GetInstance().GetFloorPrefabObjects[(int)hitScreen.GetFloorForm];
        var scale=prefab.transform.lossyScale;
        var col = prefab.GetComponent<BoxCollider>();
        scale.x *= col.size.x;
        scale.y *= col.size.y;
        scale.z *= col.size.z;

        scale *= 0.5f;

        var colliders = Physics.OverlapBox(hitPos, scale, transform.rotation);
        foreach (var i in colliders)
        {
            if (i.GetComponent<PlayerMove>())
            {
                return false;
            }
        }
        return true;
    }

    protected override void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
        SetLineRenderPosition(hit.point);
        if (!isLocalPlayer) return;
        PointerHitScreen hitScreen = hit.collider.GetComponent<PointerHitScreen>();

        //スクリーンにヒット&&床を生成できる状態でなければ削除してリターン
        if (!(hitScreen &&CanCreateFloor(hitScreen)))
        {
            DeleteFloor();
            floorPredictionActiveController.AllInactive();
            return;
        }
        var canCreateFloor = CanCreateFloor(hit.point,hitScreen);

        if (IsPressDownTrigger()&&canCreateFloor)
        {
            CreateFloor(hitScreen.GetFloorForm, hitScreen);
        }

        if (controllingFloor)
        {
            ControlFloor(hit.point, hit.normal, hitScreen);
        }
        else
        {
            floorPredictionActiveController.SetView(hit.point +hit.normal* 0.1f, hitScreen,canCreateFloor);
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

    bool IsChangedHitScreen(PointerHitScreen hitScreen)
    {
        return currentHitScreen && (hitScreen);
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
        if (!isLocalPlayer) return;
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
        CmdSetScreenStencilActive(pointerHitScreen.GetComponent<NetworkIdentity>(),true);
         currentHitScreen = pointerHitScreen;
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
        CmdSetScreenStencilActive(currentHitScreen.GetComponent<NetworkIdentity>(),false);
        currentHitScreen = null;
        GimmickFloorSpawner.GetInstance().ReleaseFloor(controllingFloor);
        controllingFloor = null;
    }

    void SetScreenStencilActive(PointerHitScreen screen,bool active)
    {
        if (screen)
        {
            var screenRenderer = screen.GetComponent<Renderer>();
            var meshRenderers = screen.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshrenderer in meshRenderers)
            {
                //スクリーン本体のrendererは無視
                if (meshrenderer == screenRenderer)
                {
                    continue;
                }
                meshrenderer.enabled = active;
            }
        }
    }

    [Command]
    void CmdSetScreenStencilActive(NetworkIdentity networkIdentity,bool active)
    {
        RpcSetScreenStencilActive(networkIdentity,active);
    }

    [ClientRpc]
    void RpcSetScreenStencilActive(NetworkIdentity networkIdentity,bool active)
    {
        Debug.Log("YOOO");
        SetScreenStencilActive(networkIdentity.GetComponent<PointerHitScreen>(), active);
    }
}
