using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBreakNetObject : GimmickBase {
    [SerializeField]
    int triggerID = 1;

    void Start () {
        m_acTriggerEnterAction += BreakNetObject;
	}
	void BreakNetObject(Collider collider)
    {
        var gimmick = collider.GetComponent<GimmickBase>();
        if (gimmick&&gimmick.GimmickID==triggerID)
        {
            gimmick.DestroyThisObject();
        }
    }
}
