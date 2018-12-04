using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GimmickIDManager {
    //被らないIDを取得
    public static int GetNotOverLapGimmickID(int needMinID = 1000)
    {
        var gimmicks = Object.FindObjectsOfType(typeof(GimmickBase));
        var IDList = new List<int>();
        foreach (GimmickBase g in gimmicks)
        {
            if (g.gameObject.activeInHierarchy)
            {
                IDList.Add(g.GimmickID);
            }
        }
        for (int i = needMinID; i < 10000; i++)
        {
            if (!IDList.Contains(i))
            {
                Debug.Log("GimmickID" + i + "で作成");
                return i;
            }
            else
            {
                Debug.Log("GimmickID" + i + "は既に存在します。");
            }
        }
        Debug.LogError("新規ギミックIDの取得に失敗しました");
        return -1;
    }
}
