using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchTVOn : SwitchActionBase
{

    public override void OnAction()
    {
        CmdTVOn();
    }

    [Command]
    void CmdTVOn()
    {
        RpcTVOn();
    }

    [ClientRpc]
    void RpcTVOn()
    {
        TVSwitch.IsOn = true;
    }

}
