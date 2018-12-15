using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickPlayerRespawn : GimmickBase
{

    private void Start()
    {
        m_acTriggerEnterAction += Respawn;
    }
    void Respawn(Collider collider)
    {
        var pStatus = collider.GetComponent<PlayerStatus>();
        if (!pStatus) return;
        RpcRespawn((int)pStatus.Number);
    }
    [ClientRpc]
    void RpcRespawn(int playerNumber)
    {
        if (PlayerManager.LocalPlayer.GetComponent<PlayerStatus>().Number == (PlayerNumber)playerNumber)
        {
            PlayerRespawner.GetInstance().RespawnLocalPlayer();
        }
    }
}
