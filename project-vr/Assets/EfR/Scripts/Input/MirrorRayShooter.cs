using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRayShooter : MonoBehaviour
{
    [SerializeField]
    Transform shooter;
    LineRenderer lineRenderer;

    [SerializeField]
    int maxReflection = 30;

    List<Vector3> lineRenderPositions = new List<Vector3>();

    [SerializeField]
    LayerMask layerMask;

    //このRayを飛ばしているGimmickBaseを持ったコライダー
    //[SerializeField]
    Collider ownerCollider;

    int pointCount = 0;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        pointCount = 0;
        lineRenderPositions.Clear();
        RecursiveShootRay(shooter.position, shooter.forward);
        ApplyLineRenderPositions();
    }
    //マックスカウントまで反射するrayを飛ばし続ける再起関数
    void RecursiveShootRay(Vector3 origin, Vector3 direction)
    {

        if (!SetPosition(origin)) return;
        pointCount++;
        RaycastHit hit;
        Ray ray = new Ray(origin, direction);
        GimmickBase gimmick = null;
        bool isHit = Physics.Raycast(ray, out hit, 1000f, layerMask);
        if (isHit)
        {
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
                RecursiveShootRay(hit.point, reflectionDir);
            }
            else
            {
                SetPosition(hit.point);
                if ((gimmick = hit.collider.GetComponent<GimmickBase>()) != null
                    && (PlayerManager.LocalPlayer.GetComponent<PlayerMove>().moveType != PlayerMove.MoveType._2D))
                {
                    gimmick.OnPointerHit(ownerCollider);
                }

            }
        }
        else
        {

            SetPosition(origin + direction * 10);
        }
    }


    bool SetPosition(Vector3 position)
    {
        lineRenderPositions.Add(position);
        return maxReflection >= lineRenderPositions.Count;
    }
    void ApplyLineRenderPositions()
    {
        lineRenderer.positionCount = lineRenderPositions.Count;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, lineRenderPositions[i]);
        }
    }
}
