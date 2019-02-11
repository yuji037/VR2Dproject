using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MeteoBossSkill : BossSkillBase {
    [SerializeField]
    Vector2 fallRange;

    [SerializeField]
    Transform fallCenter;

    [SerializeField]
    GameObject meteoPrefab;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(fallCenter.position,new Vector3(fallRange.x,0,fallRange.y));
    }

    Vector3 GetFallPos()
    {
        return fallCenter.position + new Vector3(Random.Range(-fallRange.x*0.5f,fallRange.x*0.5f),0, Random.Range(-fallRange.y*0.5f,fallRange.y*0.5f));
    }

    public override void InvokeSkill()
    {
        RpcInvokeSkill(GetFallPos());
        Debug.Log("メテオ");
    }
    
    [ClientRpc]
    void RpcInvokeSkill(Vector3 fallPos)
    {
        Instantiate(meteoPrefab);
        meteoPrefab.transform.position = fallPos;
    }

}
