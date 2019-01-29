using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickTeleporter : GimmickBase{

    [SerializeField]
    Transform teleportPos;

    [SerializeField]
    int triggerID = 1;

    [SerializeField]
    ParticleSystem teleportEffect;

    private void Start()
    {
        isCallOnlyServer = false;
        m_acTriggerEnterAction += TeleportPlayer;
    }

    void TeleportPlayer(Collider collider)
    {
        if (collider.gameObject==PlayerManager.LocalPlayer)
        {
            PlayerManager.LocalPlayer.transform.position = teleportPos.position;
            CmdPlayTeleportEffect();
        }
    }

    [Command]
    void CmdPlayTeleportEffect()
    {
        RpcPlayTeleportEffect();
    }

    [ClientRpc]
    void RpcPlayTeleportEffect()
    {
        teleportEffect.transform.position = teleportPos.position;
        teleportEffect.Play();
    }
}
