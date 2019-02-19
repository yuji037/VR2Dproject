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
        NetworkServer.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        foreach (var i in screenBreaker)
        {
            i.StartBreak();
        }
    }
}
