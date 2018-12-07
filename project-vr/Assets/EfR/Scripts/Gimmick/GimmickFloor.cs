using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public enum FloorForm
{
    Normal,
    LongX,
    LongY,
}
public class GimmickFloor : GimmickBase
{
    public FloorForm floorForm;
    //private new void Awake()
    //{
    //    base.Awake();
    //    Debug.Log(playerID);
    //}
    [ClientRpc]
    public void RpcInit(NetworkIdentity playerNetID)
    {
        if (PlayerManager.LocalPlayer.GetComponent<NetworkIdentity>() == playerNetID)
        {
            GimmickFloorSpawner.GetInstance().RegisterFloor(this);
        }
    }
}
