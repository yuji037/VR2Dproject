using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchSelectStage : SwitchActionBase
{
    [SerializeField]
    SelectStageMenu SelectStageMenu;

    public override void OnAction()
    {
        RpcGotoSelectStage();
    }
    [ClientRpc]
    void RpcGotoSelectStage()
    {
        SelectStageMenu.GoToSelectStage();
    }


}
