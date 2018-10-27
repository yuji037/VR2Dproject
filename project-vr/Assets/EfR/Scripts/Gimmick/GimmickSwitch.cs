using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickSwitch : GimmickBase {

    public enum Type {
        DOOR,
    }

    [SerializeField]
    Type m_eGimmickType;

    [SerializeField]
    int m_iTriggerGimmickID;

    [SerializeField]
    int m_iActGimmickID;

	// Use this for initialization
 	void Start () {
        m_aTriggerAction += PressAction;
	}

    protected virtual void PressAction(Collider other, int otherGimmickID)
    {
        if(otherGimmickID == m_iTriggerGimmickID)
        {
            switch ( m_eGimmickType )
            {
                case Type.DOOR:
                    var gimik = GimmickManager.GetGimmick(m_iActGimmickID);
                    var door = gimik.GetComponent<GimmickDoor>();
                    door.Open();
                    break;
            }
        }
    }
}
