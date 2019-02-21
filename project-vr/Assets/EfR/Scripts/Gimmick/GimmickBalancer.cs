using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBalancer : GimmickBase
{
    [SerializeField]
    int[] triggerIDs;

    public int RidingCount { get; private set; }
    private void Start()
    {
        m_acTriggerEnterAction += HitAction;
        m_acTriggerExitAction += ExitAction;
    }
    void HitAction(Collider collider)
    {
        if (collider.isTrigger) return;
        var gimmick = collider.GetComponent<GimmickBase>();
        if (!gimmick) return;
        HitAction(gimmick.GimmickID);
    }
    void ExitAction(Collider collider)
    {
        if (collider.isTrigger) return;
        var gimmick = collider.GetComponent<GimmickBase>();
        if (!gimmick) return;
        ExitAction(gimmick.GimmickID);
    }
    void HitAction(int id)
    {
        foreach (var i in triggerIDs)
        {
            if (id == i)
            {
                RidingCount++;
            }
        }
    }

    void ExitAction(int id)
    {
        foreach (var i in triggerIDs)
        {
            if (id == i)
            {
                RidingCount--;
                Debug.Log("抜けた");
            }
        }
    }

    //IEnumerator MoveRoutine(bool isDown)
    //{
    //    float multiPly = (isDown) ?-1.0f:1.0f;

    //    for (float t=0;t<oneMoveDuration;t+=Time.deltaTime)
    //    {
    //        transform.Translate(new Vector3(0,oneMoveDuration+oneMoveValue*multiPly,0));
    //    }

    //    yield return null;
    //}
}
