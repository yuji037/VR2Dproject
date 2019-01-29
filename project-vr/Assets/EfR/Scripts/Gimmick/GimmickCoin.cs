using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickCoin : GimmickBase {
    [SerializeField]
    int triggerID = 1;

    System.Action triggerEnterAction;
    private void Start()
    {
        m_aTriggerEnterAction += Excute;
    }
    public void SetTriggerEnterAction(System.Action action)
    {
        triggerEnterAction = action;
    }

    void Excute(int id)
    {
        if (triggerID==id)
        {
            if (triggerEnterAction != null) triggerEnterAction();
            DestroyThisObject();
        }
    }
}
