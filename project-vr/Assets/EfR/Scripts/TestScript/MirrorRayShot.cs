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
    int maxReflection=30;

    List<Vector3> lineRenderPositions=new List<Vector3>();

    [SerializeField]
    LineRenderer debugRenderer;
    // Use this for initialization
    void Start()
    {

    }
    int pointCount=0;
    // Update is called once per frame
    void Update()
    {
        pointCount = 0;
        lineRenderPositions.Clear();
        RecursiveShootRay(shooter.position,shooter.forward);
        ApplyLinRenderPositions();
    }
    //マックスカウントまで反射するrayを飛ばし続ける再起関数
    void RecursiveShootRay(Vector3 origin, Vector3 direction)
    {

        if (!SetPosition(origin)) return;
        pointCount++;
        RaycastHit hit;
        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out hit, 1000f)&& (hit.collider.GetComponent<Mirror>()))
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
        else if(lineRenderPositions.Count>1)
        {

            SetPosition(origin+direction * 10);
        }
    }

    bool SetPosition(Vector3 position)
    {
        lineRenderPositions.Add(position);
        return maxReflection >= lineRenderPositions.Count;
    }
    void ApplyLinRenderPositions()
    {
        lineRenderer.positionCount=lineRenderPositions.Count;
        for (int i=0;i<lineRenderer.positionCount;i++)
        {

            lineRenderer.SetPosition(i,lineRenderPositions[i]);
        }
    }
}
public class RayShooter
{
    //Rayを飛ばして当たったインスタンスを返す
    public static T GetHitObject<T>(Vector2 screen_pos, ref Vector3 hit_point, Camera ray_camera, LayerMask layer_mask, float range = 1000.0f)
        where T : MonoBehaviour
    {
        Ray ray = ray_camera.ScreenPointToRay(screen_pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range, layer_mask))
        {
            var hitObj = hit.transform.GetComponent<T>();
            if (!hitObj) return null;
            hit_point = hit.point;
            return hitObj;
        }
        return null;
    }
}
