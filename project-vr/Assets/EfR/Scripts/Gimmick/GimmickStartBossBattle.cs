using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickStartBossBattle : GimmickBase{
    [SerializeField]
    int triggerID=1;

    private void Start()
    {
        m_aTriggerEnterAction +=StartBossBattle;
    }

    void StartBossBattle(int id)
    {
        if (id==triggerID)
        {
            CmdStartBossBattle();
            GetComponent<Collider>().enabled = false;
        }
    }
    [Command]
    void CmdStartBossBattle()
    {
        RpcStartBossBattle();
    }
    [ClientRpc]
    void RpcStartBossBattle()
    {
        BossBattle.GetInstance().StartBossBattle();
    }
}
