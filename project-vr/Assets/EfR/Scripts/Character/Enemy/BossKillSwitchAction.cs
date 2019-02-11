using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossKillSwitchAction : SwitchActionBase {
    [SerializeField]
    BossKillPerformer bossKillPerformer;        

    public override void OnAction()
    {
        RpcKill();
    }

    [ClientRpc]
    public void RpcKill()
    {
        bossKillPerformer.Kill();
    }
}
