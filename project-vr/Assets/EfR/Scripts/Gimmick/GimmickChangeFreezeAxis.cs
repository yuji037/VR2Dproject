using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickChangeFreezeAxis : GimmickBase
{
    [System.Serializable]
    struct FreezeAxis
    {
        public bool posX;
        public bool posY;
        public bool posZ;
        public bool rotX;
        public bool rotY;
        public bool rotZ;
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
        if (!(gim && collider.GetComponent<Rigidbody>())) return;
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
        var rigid = identity.GetComponent<Rigidbody>();

        var freezeFlag =((axis.posX) ? RigidbodyConstraints.FreezePositionX : RigidbodyConstraints.None)
                      | ((axis.posY) ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.None)
                      | ((axis.posZ) ? RigidbodyConstraints.FreezePositionZ : RigidbodyConstraints.None)
                      | ((axis.rotX) ? RigidbodyConstraints.FreezeRotationX : RigidbodyConstraints.None)
                      | ((axis.rotY) ? RigidbodyConstraints.FreezeRotationY : RigidbodyConstraints.None)
                      | ((axis.rotZ) ? RigidbodyConstraints.FreezeRotationZ : RigidbodyConstraints.None);
        rigid.constraints = freezeFlag;
    }
}
