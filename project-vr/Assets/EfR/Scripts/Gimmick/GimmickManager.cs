using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : SingletonMonoBehaviour<GimmickManager> {

    Dictionary<int, GimmickBase> m_dcGimmickBases = new Dictionary<int, GimmickBase>();

    public void Register(GimmickBase gimmick)
    {
        if ( !m_dcGimmickBases.ContainsKey(gimmick.GimmickID) )
        {
            m_dcGimmickBases[gimmick.GimmickID] = gimmick;
        }
        else
        {
            // ギミックIDの重複
        }

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


    //========================================================================
    // ↓Gimmickは今後使わなくなります
    //========================================================================

    Dictionary<int, Gimmick> dictionary = new Dictionary<int, Gimmick>();

    public void Register(Gimmick gimmick)
    {
        dictionary.Add(gimmick.ID, gimmick);
        Debug.Log("こっちのGimmickは後々使わなくなります");
    }

    public void ReceiveMessage(int id, string message)
    {
        dictionary[id].SendMessage(message);
    }
}
