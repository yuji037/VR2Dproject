using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickAllSavableObjectsRespawn : GimmickBase {

    [SerializeField]
    int[] triggerGimmickIDs;
    private void Start()
    {
        m_aTriggerEnterAction += Load;
    }
    void Load(int id)
    {
        if (!ContainsTriggerID(id)) return;
        StageSaver.GetInstance().LoadAllSavableObjects();
    }
    bool ContainsTriggerID(int id)
    {
        foreach (var i in triggerGimmickIDs)
        {
            if (i==id)
            {
                return true;
            }
        }
        return false;
    }
}
