using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserPointerBase : NetworkBehaviour
{
    [SerializeField]
    Transform shooter;

    LineRenderer lineRenderer;

    List<Vector3> lineRenderPositions = new List<Vector3>();

    [SerializeField]
    LayerMask layerMask;

    protected bool isLinePoistionClear = false;
    //このRayを飛ばしているGimmickBaseを持ったコライダー
    [SerializeField]
    protected Collider ownerCollider;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderPositions.Clear();
        OnFlameStart();
        ShootRay(shooter.position, shooter.forward);
        ApplyLineRenderPositions();
    }
    //マックスカウントまで反射するrayを飛ばし続ける再起関数
    protected void ShootRay(Vector3 origin, Vector3 direction)
    {
        SetPosition(origin);
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
    }
    protected virtual void OnFlameStart()
    {
    }
    protected virtual void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
    }
    protected virtual void NoHitAction(Vector3 origin, Vector3 direction)
    {
        SetPosition(origin + direction * 10);
    }
    protected void SetPosition(Vector3 position)
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
