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

    protected Action<Collision, int> m_aCollisionEnterAction;
    protected Action<Collision, int> m_aCollisionStayAction;
    protected Action<Collision, int> m_aCollisionExitAction;
    protected Action<Collider, int> m_aTriggerEnterAction;
    protected Action<Collider, int> m_aTriggerStayAction;
    protected Action<Collider, int> m_aTriggerExitAction;

    // ※継承先でAwakeを使いたい場合はbase.Awake()を呼ぶ。
    protected virtual void Awake()
    {
        GimmickManager.GetInstance().Register(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( m_aCollisionEnterAction == null ) return;

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        m_aCollisionEnterAction(collision, gmk.GimmickID);
    }

    private void OnCollisionStay(Collision collision)
    {
        if ( m_aCollisionStayAction == null ) return;

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        m_aCollisionStayAction(collision, gmk.GimmickID);
    }

    private void OnCollisionExit(Collision collision)
    {
        if ( m_aCollisionExitAction == null ) return;

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        m_aCollisionExitAction(collision, gmk.GimmickID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( m_aTriggerEnterAction == null ) return;

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        m_aTriggerEnterAction(other, gmk.GimmickID);
    }

    private void OnTriggerStay(Collider other)
    {
        if ( m_aTriggerStayAction == null ) return;

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        m_aTriggerStayAction(other, gmk.GimmickID);
    }

    private void OnTriggerExit(Collider other)
    {
        if ( m_aTriggerExitAction == null ) return;

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        m_aTriggerExitAction(other, gmk.GimmickID);
    }
}
