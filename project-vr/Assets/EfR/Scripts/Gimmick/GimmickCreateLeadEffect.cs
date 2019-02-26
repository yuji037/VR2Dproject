using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickCreateLeadEffect : GimmickBase{
    [SerializeField]
    int triggerID = 1;
    [SerializeField]
    StagePathLeader stagePathLeader;
	// Use this for initialization
	void Start () {
        //m_aTriggerEnterAction += HitAction;
        m_aPointerHitAction += HitAction;
	}
	void HitAction(int id)
    {
        if (id==triggerID)
        {
            stagePathLeader.CreateLeadEffect(transform.position);
        }
    } 
}
