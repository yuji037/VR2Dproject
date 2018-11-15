using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickSwitch : GimmickBase {

    public enum Type {
        DOOR,
        BUTTON,
        ACTIVE,
    }

    [SerializeField]
    Type m_eGimmickType;

    [SerializeField]
    int[] m_iTriggerGimmickIDs;

    [SerializeField]
    int m_iActorGimmickID;

    [SerializeField]
    bool m_IsActOnRelease;

    // Use this for initialization
    void Start()
    {
        m_aTriggerEnterAction += PressAction;
        m_aPointerHitAction += PressAction;

        if(m_IsActOnRelease) m_aTriggerExitAction += ReleaseAction;
    }

    protected virtual void PressAction(int otherGimmickID)
    {
        foreach ( var triggerID in m_iTriggerGimmickIDs )
        {
            if ( otherGimmickID == triggerID )
            {
                var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
                switch ( m_eGimmickType )
                {
                    case Type.DOOR:
                        var door = gimik as GimmickDoor;
                        door.Open();
                        break;

                    case Type.BUTTON:
                        var blockButton = gimik as GimmickButton;
                        blockButton.OnTrigger();
                        break;

                    case Type.ACTIVE:
                        gimik.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    protected virtual void ReleaseAction(int otherGimmickID)
    {
        foreach ( var triggerID in m_iTriggerGimmickIDs )
        {
            if ( otherGimmickID == triggerID )
            {
                var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
                switch ( m_eGimmickType )
                {
                    case Type.DOOR:
                        break;

                    case Type.BUTTON:
                        var blockButton = gimik as GimmickButton;
                        if ( !blockButton.GetIsTggle() ) { blockButton.OffTrigger(); }
                        break;

                    case Type.ACTIVE:
                        gimik.gameObject.SetActive(false);
                        break;
                }
            }
        }
    }
}
