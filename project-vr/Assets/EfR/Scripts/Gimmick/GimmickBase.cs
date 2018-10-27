using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GimmickBase : MonoBehaviour {

    [SerializeField]
    private int m_iGimmickID;
    public int GimmickID
    {
        get { return m_iGimmickID; }
    }

    protected Action<Collision, int> m_aCollisionAction;
    protected Action<Collider, int> m_aTriggerAction;


    // ※継承先でAwakeを使いたい場合はbase.Awake()を呼ぶ。
    protected virtual void Awake()
    {
        GimmickManager.GetInstance().Register(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突相手のGimmickIDを取得
        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        if ( m_aCollisionAction != null )
            m_aCollisionAction(collision, gmk.GimmickID);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 衝突相手のGimmickIDを取得
        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        if ( m_aTriggerAction != null )
            m_aTriggerAction(other, gmk.GimmickID);
    }
}
