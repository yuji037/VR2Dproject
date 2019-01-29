using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickButton : GimmickBase {
    [SerializeField]
    int triggerID;
    bool isPushd;

    [SerializeField]
    SwitchActionBase switchAction;

    public System.Func<bool> pushEndCondition;
    // Use this for initialization
	void Start () {
        m_aTriggerEnterAction += Pushd;
	}

    void Pushd(int id)
    {
        if (id==triggerID&&!isPushd)
        {
            isPushd = true;
            switchAction.OnAction();
            transform.Translate(new Vector3(0, -transform.lossyScale.y * 0.9f, 0));
        }
    }

    public void UnPush()
    {
        isPushd = false;
        switchAction.OffAction();
        transform.Translate(new Vector3(0, transform.lossyScale.y * 0.9f, 0));
    }

}
