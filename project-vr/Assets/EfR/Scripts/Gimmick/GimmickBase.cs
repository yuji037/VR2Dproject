using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public abstract class GimmickBase : NetworkBehaviour {

    [SerializeField]
    private int m_iGimmickID;
    public int GimmickID
    {
        get { return m_iGimmickID; }
    }

    protected Action<int> m_aCollisionEnterAction;
    //protected Action<int> m_aCollisionStayAction;
    protected Action<int> m_aCollisionExitAction;
    protected Action<int> m_aTriggerEnterAction;
    //protected Action<int> m_aTriggerStayAction;
    protected Action<int> m_aTriggerExitAction;
    protected Action<int> m_aPointerHitAction;

    [ClientRpc]void RpcCollisionEnterAction(int otherGimmickID  ) {  m_aCollisionEnterAction(    otherGimmickID);     }
    [ClientRpc]void RpcCollisionExitAction (int otherGimmickID  ) {  m_aCollisionExitAction(     otherGimmickID);     }
    [ClientRpc]void RpcTriggerEnterAction  (int otherGimmickID  ) {  m_aTriggerEnterAction(      otherGimmickID);     }
    [ClientRpc]void RpcTriggerExitAction   (int otherGimmickID  ) {  m_aTriggerExitAction(       otherGimmickID);     }
    [ClientRpc]void RpcPointerHitAction    (int pointerGimmickID) {  m_aPointerHitAction(        pointerGimmickID);   }
    //[RPC] void ActCollisionStayAction (int otherGimmickID  ) {  m_aCollisionStayAction(     otherGimmickID);     }
    //[RPC] void ActTriggerStayAction   (int otherGimmickID  ) {  m_aTriggerStayAction(       otherGimmickID);     }

    // ※継承先でAwakeを使いたい場合はbase.Awake()を呼ぶ。
    protected virtual void Awake()
    {
        if ( m_iGimmickID != -1 )
            GimmickManager.GetInstance().Register(this);

    }

    public override void OnStartServer()
    {
        if ( GimmickID >= 100 )
            NetworkServer.Spawn(gameObject);
        //Debug.Log("NetworkServer spawn gimmick : " + gameObject.name);
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if ( m_aCollisionEnterAction == null ) return;

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        RpcCollisionEnterAction(gmk.GimmickID);
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if ( m_aCollisionStayAction == null ) return;

    //    var gmk = collision.gameObject.GetComponent<GimmickBase>();
    //    if ( !gmk ) return;

    //    //m_aCollisionStayAction(gmk.GimmickID);
    //    networkView.RPC("ActCollisionStayAction", RPCMode.All, gmk.GimmickID);
    //}

    [ServerCallback]
    private void OnCollisionExit(Collision collision)
    {
        if ( m_aCollisionExitAction == null ) return;
        if ( !isServer ) return;

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        RpcCollisionExitAction(gmk.GimmickID);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if ( m_aTriggerEnterAction == null ) return;
        if ( !isServer ) return;

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        RpcTriggerEnterAction(gmk.GimmickID);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if ( m_aTriggerStayAction == null ) return;

    //    var gmk = other.gameObject.GetComponent<GimmickBase>();
    //    if ( !gmk ) return;

    //    //m_aTriggerStayAction(gmk.GimmickID);
    //    networkView.RPC("ActTriggerStayAction", RPCMode.All, gmk.GimmickID);
    //}

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        if ( m_aTriggerExitAction == null ) return;
        if ( !isServer ) return;

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if ( !gmk ) return;

        RpcTriggerExitAction(gmk.GimmickID);
    }

    [ServerCallback]
    public void OnPointerHit(Collider rayShooter)
    {
        if (m_aPointerHitAction == null) return;
        if ( !isServer ) return;

        var gmk = rayShooter.gameObject.GetComponent<GimmickBase>();
        if (!gmk) return;

        RpcPointerHitAction(gmk.GimmickID);
    }
}
