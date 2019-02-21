using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BreakableObject : NetworkBehaviour {
    [SerializeField]
    ScreenBreaker[] screenBreaker;
    private void Start()
    {
        foreach (var i in screenBreaker)
        {
            i.transform.parent = transform.parent;
        }
    }
    public void Break()
    {
        if (!isServer) return;
        RpcBreak();
    }
    [ClientRpc]
    public void RpcBreak()
    {
        foreach (var i in screenBreaker)
        {
            i.StartBreak();
        }
        if(isServer)NetworkServer.Destroy(gameObject);
    }

}
