using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickRocket : GimmickBase {

    [SerializeField]
    int triggerID = 1;
	// Use this for initialization
	void Start () {
        m_aTriggerEnterAction += Suicide;
	}
	void Suicide(int id)
    {
        if (id==triggerID)
        {

        }
    }
	
}
