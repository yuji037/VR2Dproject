using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BeamBossSkill : BossSkillBase
{
    [SerializeField]
    GameObject beamPrefab;

    [SerializeField]
    Transform launchPos;

    public override void InvokeSkill()
    {
        Debug.Log("ビーム");
        var pPos=PlayerManager.LocalPlayer.transform.position;
        var lPos = launchPos.position;
        pPos.y = 0;
        lPos.y = 0;
        var dir=pPos - lPos;
        RpcInvokeSkill(dir.normalized); 
    }

    [ClientRpc]
    public void RpcInvokeSkill(Vector3 direction)
    {
        var obj=Instantiate(beamPrefab);
        obj.transform.position = launchPos.position;
        obj.transform.forward = direction;
    }
}
