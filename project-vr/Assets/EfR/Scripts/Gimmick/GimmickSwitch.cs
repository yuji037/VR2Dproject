using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickSwitch : GimmickBase {

    public enum Type {
        DOOR,
        ACTIVE,
    }

    [SerializeField]
    Type m_eGimmickType;

    [SerializeField]
    int m_iTriggerGimmickID;

    [SerializeField]
    int m_iActorGimmickID;

    [SerializeField]
    bool m_IsActOnRelease;

    // Use this for initialization
    void Start()
    {
        m_aTriggerEnterAction += PressAction;
        if(m_IsActOnRelease) m_aTriggerExitAction += ReleaseAction;
    }

    protected virtual void PressAction(Collider other, int otherGimmickID)
    {
        if ( otherGimmickID == m_iTriggerGimmickID )
        {
            var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
            switch ( m_eGimmickType )
            {
                case Type.DOOR:
                    var door = gimik as GimmickDoor;
                    door.Open();
                    break;

                case Type.ACTIVE:
                    gimik.gameObject.SetActive(true);
                    break;
            }
        }
    }

    protected virtual void ReleaseAction(Collider other, int otherGimmickID)
    {
        if ( otherGimmickID == m_iTriggerGimmickID )
        {
            var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
            switch ( m_eGimmickType )
            {
                case Type.DOOR:
                    break;

                case Type.ACTIVE:
                    gimik.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
