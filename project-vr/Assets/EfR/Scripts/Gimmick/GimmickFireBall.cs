using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickFireBall : GimmickBase{
    [SerializeField]
    int triggerID = 5;

    [SerializeField]
    LayerMask layerMask;
    
    float radius;

	// Use this for initialization
	void Start () {
        m_acTriggerEnterAction += OnHit;
        radius = GetComponent<SphereCollider>().radius * transform.lossyScale.x;
    }
    private void Update()
    {
        if (Physics.CheckSphere(transform.position, radius, layerMask))
        {
            DestroyThisObject();
        }
    }
    private void OnHit(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        var gimmick = collider.GetComponent<GimmickBase>();
        if (!gimmick) return;
            var pm=gimmick.GetComponent<PlayerMove>();
        if (gimmick.GimmickID==triggerID)
        {
            gimmick.DestroyThisObject();
            DestroyThisObject();
        }
        else if(pm)
        {
            pm.RpcRespawn();
            DestroyThisObject();
        }
    }
}
