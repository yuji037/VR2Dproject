using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchChangeActive : SwitchActionBase {

    [SerializeField]
    GameObject activateTarget;

    [SerializeField]
    bool targetDefaultActive;

    public override List<Transform> ActorObjects
    {
        get
        {
            return new List<Transform>() {activateTarget.transform};
        }
    }

    private void Start()
    {
        activateTarget.SetActive(targetDefaultActive);
    }

    [Command]
    void CmdSetActive(bool active)
    {
        RpcSetActive(active);
    }

    [ClientRpc]
    void RpcSetActive(bool active)
    {
        activateTarget.SetActive(active);
    }

    public override void OnAction()
    {
        activateTarget.SetActive(!targetDefaultActive);
    }

    public override void OffAction()
    {
        activateTarget.SetActive(targetDefaultActive);
    }
    
}
