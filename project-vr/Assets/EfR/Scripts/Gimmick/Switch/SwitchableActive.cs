using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchableActive : MonoBehaviour,ISwitchableObject {

    [SerializeField]
    bool defaultActive;

    public void Start()
    {
        gameObject.SetActive(defaultActive);
    }

    public void OnAction()
    {
        gameObject.SetActive(!defaultActive);
    }

    public void OffAction()
    {
        gameObject.SetActive(defaultActive);
    }

    //[Command]
    //void CmdSetActive(bool active)
    //{
    //    RpcSetActive(active);
    //}

    //[ClientRpc]
    //void RpcSetActive(bool active)
    //{
    //    TargetSetActive(active);
    //}

    //void TargetSetActive(bool active)
    //{
    //    gameObject.SetActive(active);
    //}
}
