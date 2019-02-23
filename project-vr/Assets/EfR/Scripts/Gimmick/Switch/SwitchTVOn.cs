using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchTVOn : SwitchActionBase
{
    private void Start()
    {
        if (isServer)
        {
            DebugTools.RegisterDebugAction(KeyCode.Q,CmdTVOn,"ゲーム電源ON");
        }
    }

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
        TVSwitch.IsOn = !TVSwitch.IsOn;
    }

}
