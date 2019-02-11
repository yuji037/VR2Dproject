using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickBossHeart : GimmickBase{
    [SerializeField]
    int triggerID = 1;

    [SerializeField]
    BossLeg linkLeg;

    LineRenderer lineRenderer;

    private void Start()
    {
        m_aTriggerEnterAction += Excute;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPositions(new Vector3[] {transform.position,linkLeg.transform.position });
    }

    void Excute(int id)
    {
        if (id==triggerID)
        {
            CmdGiveDamage();
        }
    }

    [Command]
    void CmdGiveDamage()
    {
        RpcGiveDamage();
    }
    [ClientRpc]
    void RpcGiveDamage()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        lineRenderer.enabled = false ;
        //linkLeg.Death();
    }
}
