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
        m_acTriggerEnterAction += HitAction;
        isCallOnlyServer = false;
	}
	void HitAction(Collider collider)
    {
        if (collider.gameObject==PlayerManager.LocalPlayer&&
            PlayerManager.playerMove.moveType==PlayerMove.MoveType.FIXED)
        {
            stagePathLeader.CreateLeadEffect(transform.position);
        }
    } 
}
