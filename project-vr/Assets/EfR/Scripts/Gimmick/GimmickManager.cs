using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : SingletonMonoBehaviour<GimmickManager> {

    Dictionary<int, IActor> m_dcGimmickBases = new Dictionary<int, IActor>();

    public void Register(IActor gimmick)
    {
        if ( !m_dcGimmickBases.ContainsKey(gimmick.GetID()) )
        {
            m_dcGimmickBases[gimmick.GetID()] = gimmick;
        }
        else
        {
            // ギミックIDの重複
        }

    }

    public void Unregister(IActor gimmick)
    {
        if (m_dcGimmickBases.ContainsKey(gimmick.GetID()))
        {
            m_dcGimmickBases.Remove(gimmick.GetID());
        }
    }

    public static IActor GetActor(int gimmickID)
    {
        var gmInstance = GetInstance();

        if ( gmInstance.m_dcGimmickBases.ContainsKey(gimmickID) )
        {
            return gmInstance.m_dcGimmickBases[gimmickID];
        }
        Debug.LogError("不正なIDです。そのギミックIDは登録されていません。ID : " + gimmickID);
        return null;
    }
}
