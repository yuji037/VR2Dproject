using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchChangeActive : SwitchActionBase
{

    [SerializeField]
    GameObject activateTarget;

    [SerializeField]
    bool targetDefaultActive;

    public override List<Transform> ActorObjects
    {
        get
        {
            if (activateTarget)
            {
                return new List<Transform>() { activateTarget.transform };
            }
            else
            {
                return null;
            }
        }
    }

    public void Start()
    {
        TargetSetActive(targetDefaultActive);
    }

    public override void OnAction()
    {
        CmdSetActive(!targetDefaultActive);
    }

    public override void OffAction()
    {
        CmdSetActive(targetDefaultActive);
    }

    [Command]
    void CmdSetActive(bool active)
    {
        RpcSetActive(active);
    }

    [ClientRpc]
    void RpcSetActive(bool active)
    {
        TargetSetActive(active);
    }

    void TargetSetActive(bool active)
    {
        activateTarget.SetActive(active);
    }
}
