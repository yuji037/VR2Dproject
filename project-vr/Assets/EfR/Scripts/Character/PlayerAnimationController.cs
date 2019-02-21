using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAnimationController : NetworkBehaviour{

    Animator animator;
	// Use this for initialization
	void Start () {
        animator=GetComponentInChildren<Animator>();
	}
    [Command]
    public void CmdSetBool(string name, bool isPlay)
    {
        RpcSetBool(name,isPlay);
    }

    [ClientRpc]
    void RpcSetBool(string name, bool isPlay)
    {
        animator.SetBool(name,isPlay);
    }

    [Command]
    public void CmdSetFloat(string name,float value)
    {
        RpcSetFloat(name,value);
    }

    [ClientRpc]
    void RpcSetFloat(string name, float value)
    {
        if (animator)
            animator.SetFloat(name, value);
    }

}
