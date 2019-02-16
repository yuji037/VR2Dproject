using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickBossKill : GimmickBase{
    [SerializeField]
    BossKillPerformer bossKillPerformer;

    [SerializeField]
    int triggerID = 0;
    // Use this for initialization
	void Start () {
        m_aTriggerEnterAction += HitAction;
	}
    private void HitAction(int id)
    {
        if (id==triggerID)
        {
            RpcBossKill();
        }
    }

    [ClientRpc]
    void RpcBossKill()
    {
        bossKillPerformer.Kill();
    }
}
