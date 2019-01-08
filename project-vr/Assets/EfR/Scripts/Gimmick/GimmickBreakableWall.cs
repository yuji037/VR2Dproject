using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickBreakableWall : GimmickBase {

    [SerializeField]
    GameObject breakObject;

    [SerializeField]
    float breakPower;

    bool isBroken=false;
    private void Start()
    {
        m_acCollisionEnterAction += Break;
    }
    private void Break(Collision collider)
    {
        var wallBreaker = collider.transform.GetComponent<GimmickWallBreaker>();
        if (!isBroken&&wallBreaker/*&& wallBreaker.GetComponent<Rigidbody>().velocity.sqrMagnitude > breakPower*/)
        {
            RpcBreakWall();
            isBroken = true;
        }
    }
    [ClientRpc]
    void RpcBreakWall()
    {
        breakObject.SetActive(false);
    }
}
