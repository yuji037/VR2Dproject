using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserPointerMirror : LaserPointerBase
{
    [SerializeField]
    int maxReflection = 30;
    int reflectionCount = 0;

    private void Awake()
    {
        isLinePoistionClear = true;
    }
    protected override void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
        reflectionCount++;
        if (hit.collider.GetComponent<Mirror>())
        {

            //発射地点から衝突点までのベクトル
            var rayVec = hit.point - origin;
            //法線の長さ
            var a = Vector3.Dot(-rayVec, hit.normal);
            //Hitした平面の平行ベクトル
            var hitHorizontalVec = rayVec + hit.normal * a;
            var reflectionPoint = origin + hitHorizontalVec * 2;
            var reflectionDir = (reflectionPoint - hit.point).normalized;
            ShootRay(hit.point, reflectionDir);
        }
        else
        {
            SetLineRenderPosition(hit.point);
            GimmickBase gimmick=null;
            if ((gimmick = hit.collider.GetComponent<GimmickBase>()) != null
                && (PlayerManager.LocalPlayer.GetComponent<PlayerMove>().moveType != PlayerMove.MoveType._2D))
            {
                gimmick.OnPointerHit(ownerCollider);
            }
        }
    }
    protected override void OnFlameStart()
    {
        reflectionCount=0;
    }
}
