﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwitchAction  {
    public enum Type
    {
        DOOR,
        BUTTON,
        ACTIVE,
    }

    [SerializeField, Header("押したときの影響先ギミックID")]

    int m_iActorGimmickID;

    [SerializeField, Header("スイッチを押したときの影響先タイプ")]

    Type m_eGimmickType;

    public virtual void OnAction()
    {
        Debug.Log("Switch On");
        var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
        switch (m_eGimmickType)
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

    public virtual void OffAction()
    {
        Debug.Log("Switch Off");
        var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
        switch (m_eGimmickType)
        {
            case Type.DOOR:
                var door = gimik as GimmickDoor;
                door.Close();
                break;

            case Type.ACTIVE:
                gimik.gameObject.SetActive(false);
                break;
        }
    }
}
