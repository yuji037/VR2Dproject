using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserPointerBase : NetworkBehaviour
{
    [SerializeField]
    Transform shooter;

    [SerializeField]
    LineRenderer lineRenderer;

    List<Vector3> lineRenderPositions = new List<Vector3>();

    [SerializeField]
    LayerMask layerMask;

    protected bool isLinePoistionClear = false;

    //このRayを飛ばしているGimmickBaseを持ったコライダー
    protected Collider ownerCollider;

    protected bool isFixPointer;

    Vector3 preShooterPos;
    Vector3 preShooterForward;

    GimmickBase preHitGimmick;
    VRHand vrHand;
    private void Start()
    {
        ownerCollider = GetComponent<Collider>();
        vrHand= GetComponent<VRHand>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lineRenderer)lineRenderPositions.Clear();
        if (vrHand.playerNumber == -1 ||
            (PlayerManager.Players[vrHand.playerNumber] &&
            PlayerManager.Players[vrHand.playerNumber].GetComponent<PlayerMove>().moveType == PlayerMove.MoveType._2D))
        {
            lineRenderer.SetPosition(0,shooter.position);
            lineRenderer.SetPosition(1,shooter.position);
            return;
        }

        OnFlameStart();
        if (isFixPointer)
        {
            ShootRay(preShooterPos,preShooterForward);
        }
        else
        {
            ShootRay(shooter.position, shooter.forward);
            preShooterPos = shooter.position;
            preShooterForward = shooter.forward;
        }
        if(lineRenderer)ApplyLineRenderPositions();
    }

    protected void ShootRay(Vector3 origin, Vector3 direction)
    {
        SetLineRenderPosition(origin);
        RaycastHit hit;
        Ray ray = new Ray(origin, direction);
        bool isHit = Physics.Raycast(ray, out hit, 1000f, layerMask);
        if (isHit)
        {
            HitAction(hit, origin, direction);
        }
        else
        {
            NoHitAction(origin, direction);
        }
        CheckHitGimmick(hit,isHit);
    }
    protected virtual void OnFlameStart()
    {
    }
    protected virtual void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
    
    }
    protected virtual void NoHitAction(Vector3 origin, Vector3 direction)
    {
        SetLineRenderPosition(origin + direction * 10);
    }


    void CheckHitGimmick(RaycastHit hit,bool isHit)
    {
        if (!isLocalPlayer) return;

        GimmickBase hitGimmick = (isHit)?hitGimmick= hit.collider.GetComponent<GimmickBase>():null;

        //Exit処理
        if (preHitGimmick &&
            (!isHit||preHitGimmick!=hitGimmick))
        {
            preHitGimmick.OnPointerExit(ownerCollider);
        }

        //Hit処理
        if (hitGimmick && hitGimmick!=preHitGimmick&&
            (PlayerManager.playerMove.moveType != PlayerMove.MoveType._2D))
        {
            hitGimmick.OnPointerEnter(ownerCollider);
        }
        preHitGimmick = hitGimmick;
    }

    protected void SetLineRenderPosition(Vector3 position)
    {
        lineRenderPositions.Add(position);
    }
    void ApplyLineRenderPositions()
    {
        if (isLinePoistionClear)
        {
            lineRenderer.positionCount = lineRenderPositions.Count;
        }
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, lineRenderPositions[i]);
        }
    }
}
