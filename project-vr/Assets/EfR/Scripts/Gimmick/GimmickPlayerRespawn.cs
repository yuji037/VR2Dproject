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
        CmdRespawn((int)pStatus.Number);
    }
    [Command]
    void CmdRespawn(int playerNumber)
    {
        RpcRespawn(playerNumber);
    }
    [ClientRpc]
    void RpcRespawn(int playerNumber)
    {
        Debug.Log("LocalRespawn");
        if (PlayerManager.LocalPlayer.GetComponent<PlayerStatus>().Number == (PlayerNumber)playerNumber)
        {
            PlayerRespawner.GetInstance().RespawnLocalPlayer();
        }
    }
}
