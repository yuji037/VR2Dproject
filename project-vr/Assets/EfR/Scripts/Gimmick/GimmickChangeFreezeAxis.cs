using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickChangeFreezeAxis : GimmickBase
{
    [System.Serializable]
    struct FreezeAxis
    {
       public bool x;
       public bool y;
       public bool z;
    }
    [SerializeField]
    int[] triggerIDs;

    [SerializeField]
    FreezeAxis axis;
    // Use this for initialization
    void Start()
    {
        m_acTriggerEnterAction += HitAction;
    }

    void HitAction(Collider collider)
    {
        var gim = collider.GetComponent<GimmickBase>();
        if (!(gim&&collider.GetComponent<Rigidbody>())) return;
        foreach (var i in triggerIDs)
        {
            if (gim.GimmickID == i)
            {
                CmdHitAction(gim.GetComponent<NetworkIdentity>());
            }
        }
    }
    [Command]
    void CmdHitAction(NetworkIdentity identity)
    {
        RpcHitAction(identity);
    }

    [ClientRpc]
    void RpcHitAction(NetworkIdentity identity)
    {
        var rigid =identity.GetComponent<Rigidbody>();

        var freezeFlag =
                      ((axis.x) ? RigidbodyConstraints.FreezePositionX : RigidbodyConstraints.None)
                      | ((axis.y) ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.None)
                      | ((axis.z) ? RigidbodyConstraints.FreezePositionZ : RigidbodyConstraints.None);
        rigid.constraints = freezeFlag;
    }
}
