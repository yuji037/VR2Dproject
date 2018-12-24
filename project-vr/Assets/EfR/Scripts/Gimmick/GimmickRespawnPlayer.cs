using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickRespawnPlayer : GimmickBase
{

    private void Start()
    {
        m_acTriggerEnterAction += Respawn;
        isCallingWithServer = false;
    }
    void Respawn(Collider collider)
    {
        if (collider.gameObject == PlayerManager.LocalPlayer)
        {
            PlayerRespawner.GetInstance().RespawnLocalPlayer();
        }
    }
}
