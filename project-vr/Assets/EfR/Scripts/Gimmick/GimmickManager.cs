using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : SingletonMonoBehaviour<GimmickManager> {

    Dictionary<int, GimmickBase> m_dcGimmickBases = new Dictionary<int, GimmickBase>();

    public void Register(GimmickBase gimmick)
    {
        m_dcGimmickBases.Add(gimmick.GimmickID, gimmick);
    }

    public static GimmickBase GetGimmick(int gimmickID)
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
