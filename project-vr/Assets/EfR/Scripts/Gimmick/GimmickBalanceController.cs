using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBalanceController : MonoBehaviour
{
    [SerializeField]
    GimmickBalancer leftGimmickBalancer;

    [SerializeField]
    GimmickBalancer rightGimmickBalancer;

    [SerializeField]
    float moveYRange;

    [SerializeField]
    int maxSubCount;

    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    float oneFlameMove = 0.05f;
    float defaultY;

    private void OnDrawGizmos()
    {
        var topLeft = leftGimmickBalancer.transform.position;
        var topRight = rightGimmickBalancer.transform.position;
        topLeft.y = transform.position.y + moveYRange;
        topRight.y = transform.position.y + moveYRange;

        var botLeft = leftGimmickBalancer.transform.position;
        var botRight = rightGimmickBalancer.transform.position;
        botLeft.y = transform.position.y - moveYRange;
        botRight.y = transform.position.y - moveYRange;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(botLeft, botRight);

    }
    // Use this for initialization
    void Start()
    {
        defaultY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!leftGimmickBalancer.isServer) return;
        var subCount = leftGimmickBalancer.RidingCount - rightGimmickBalancer.RidingCount;
        if (subCount > maxSubCount)
        {
            subCount = maxSubCount;
            Debug.Log("+補正");
        }
        else if (subCount < -maxSubCount)
        {
            subCount = -maxSubCount;
            Debug.Log("-補正");
        }

        if (subCount !=0)
        {
            var toLeftPos = leftGimmickBalancer.transform.position;
            toLeftPos.y = defaultY - moveYRange * ((float)subCount / (float)maxSubCount);
            leftGimmickBalancer.transform.position = Vector3.Lerp(leftGimmickBalancer.transform.position, toLeftPos, oneFlameMove);

            var toRightPos = rightGimmickBalancer.transform.position;
            toRightPos.y = defaultY + moveYRange * ((float)subCount / (float)maxSubCount);
            rightGimmickBalancer.transform.position = Vector3.Lerp(rightGimmickBalancer.transform.position, toRightPos, oneFlameMove);
        }
        else
        {
            var toLeftPos = leftGimmickBalancer.transform.position;
            toLeftPos.y = defaultY;
            leftGimmickBalancer.transform.position = Vector3.Lerp(leftGimmickBalancer.transform.position, toLeftPos, oneFlameMove);

            var toRightPos = rightGimmickBalancer.transform.position;
            toRightPos.y = defaultY;
            rightGimmickBalancer.transform.position = Vector3.Lerp(rightGimmickBalancer.transform.position, toRightPos, oneFlameMove);

        }
        Debug.Log("サブカウント :"+ subCount);
        lineRenderer.SetPosition(0, rightGimmickBalancer.transform.position);
        lineRenderer.SetPosition(1, leftGimmickBalancer.transform.position);

    }
}
