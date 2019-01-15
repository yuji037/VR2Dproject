using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkStageNameStorage :  NetworkBehaviour{
    [Command]
    public void CmdSetCurrentStageName(int playerNum, string stageName)
    {
        RpcSetCurrentStageName(playerNum, stageName);
    }
    [ClientRpc]
    void RpcSetCurrentStageName(int playerNum, string stageName)
    {
        EFRNetworkManager.curretStageName[playerNum] = stageName;
    }
    public static NetworkStageNameStorage instance;
    public void Start()
    {
        instance = this;
    }
}
