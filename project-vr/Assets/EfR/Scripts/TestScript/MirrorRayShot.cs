using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRayShot : MonoBehaviour
{
    [SerializeField]
    Transform shooter;

    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    int maxReflection = 30;

    List<Vector3> lineRenderPositions = new List<Vector3>();

    [SerializeField]
    LayerMask layerMask;

    //このRayを飛ばしているGimmickBaseを持ったコライダー
    [SerializeField]
    Collider ownerCollider;

    int pointCount = 0;
    // Update is called once per frame
    void Update()
    {
        pointCount = 0;
        lineRenderPositions.Clear();
        RecursiveShootRay(shooter.position, shooter.forward);
        ApplyLinRenderPositions();
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
            else if ((gimmick = hit.collider.GetComponent<GimmickBase>()) != null)
            {
                SetPosition(hit.point);
                gimmick.OnPointerHit(ownerCollider);
            }
            else
            {
                SetPosition(hit.point);
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
    void ApplyLinRenderPositions()
    {
        lineRenderer.positionCount = lineRenderPositions.Count;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {

            lineRenderer.SetPosition(i, lineRenderPositions[i]);
        }
    }
}
