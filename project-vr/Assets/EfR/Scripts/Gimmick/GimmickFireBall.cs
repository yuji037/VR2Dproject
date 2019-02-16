using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickFireBall : GimmickBase{
    [SerializeField]
    int triggerID = 5;

    [SerializeField]
    LayerMask layerMask;
    
    float radius;

    float timer = 0f;

    [SerializeField]
    float deathTime=10.0f;
	// Use this for initialization
	void Start () {
        m_acTriggerEnterAction += OnHit;
        radius = GetComponent<SphereCollider>().radius * transform.lossyScale.x;
    }
    private void Update()
    {
        if (!isServer) return;
        timer += Time.deltaTime;
        if (Physics.CheckSphere(transform.position, radius, layerMask)||deathTime<=timer)
        {
            DestroyThisObject();
        }
    }
    private void OnHit(Collider collider)
    {
        var gimmick = collider.GetComponent<GimmickBase>();
        if (!gimmick) return;
        var pm=gimmick.GetComponent<PlayerMove>();
        if (gimmick.GimmickID==triggerID)
        {
            gimmick.DestroyThisObject();
            Debug.Log("aaa");
            DestroyThisObject();
        }
        else if(pm)
        {
            pm.RpcRespawn();
            DestroyThisObject();
        }
    }
}
