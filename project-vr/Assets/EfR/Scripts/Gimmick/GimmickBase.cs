using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public abstract class GimmickBase : NetworkBehaviour
{

    [SerializeField]
    private int m_iGimmickID = -1;
    public int GimmickID
    {
        get { return m_iGimmickID; }
    }
#if UNITY_EDITOR
    public void SetGimmickID(int gimmickID)
    {
        m_iGimmickID = gimmickID;
    }

#endif
    protected Action<int> m_aCollisionEnterAction;
    protected Action<int> m_aCollisionExitAction;
    protected Action<int> m_aTriggerEnterAction;
    protected Action<int> m_aTriggerExitAction;
    protected Action<int> m_aPointerHitAction;

    protected Action<Collision> m_acCollisionEnterAction;
    protected Action<Collision> m_acCollisionExitAction;
    protected Action<Collider> m_acTriggerEnterAction;
    protected Action<Collider> m_acTriggerExitAction;

    // サーバーから全クライアントで呼び出す
    // →ということは各クライアントでDoor.Open()などが起こるのでほんとは[Server]だけで起こるのが正しい？
    // それか呼び出し元が[ServerCallback]でServerのみで呼ばれるのでこちらにAttributeは要らない？
    void RpcCollisionEnterAction(int otherGimmickID)    { if(m_aCollisionEnterAction != null)   m_aCollisionEnterAction(otherGimmickID);}
    void RpcCollisionExitAction(int otherGimmickID)     { if(m_aCollisionExitAction!=null)      m_aCollisionExitAction(otherGimmickID); }
    void RpcTriggerEnterAction(int otherGimmickID)      { if(m_aTriggerEnterAction!=null)       m_aTriggerEnterAction(otherGimmickID);  }
    void RpcTriggerExitAction(int otherGimmickID)       { if(m_aTriggerExitAction != null)      m_aTriggerExitAction(otherGimmickID);   }
    void RpcPointerHitAction(int pointerGimmickID)      { if(m_aPointerHitAction!=null)         m_aPointerHitAction(pointerGimmickID);  }

    void RpcCollisionEnterAction(Collision other)       { if(m_acCollisionEnterAction!=null)m_acCollisionEnterAction(other);}
    void RpcCollisionExitAction(Collision other)        { if(m_acCollisionExitAction!=null) m_acCollisionExitAction(other); }
    void RpcTriggerEnterAction(Collider other)          { if(m_acTriggerEnterAction!=null)  m_acTriggerEnterAction(other);  }
    void RpcTriggerExitAction(Collider other)           { if(m_acTriggerExitAction!=null)   m_acTriggerExitAction(other);   }

    //サーバー上でのみ呼び出す時true
    protected bool isCallingWithServer=true;

    // ※継承先でAwakeを使いたい場合はbase.Awake()を呼ぶ。
    protected virtual void Awake()
    {
        if (m_iGimmickID != -1)
            GimmickManager.GetInstance().Register(this);

    }

    public override void OnStartServer()
    {
        if (GimmickID >= 100)
        {
            //NetworkServer.Spawn(gameObject);
            Debug.Log("NetworkServer spawn gimmick : " + gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!CanCall()) return;

        RpcCollisionEnterAction(collision);

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if (gmk) RpcCollisionEnterAction(gmk.GimmickID);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!CanCall()) return;

        RpcCollisionExitAction(collision);

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if (gmk) RpcCollisionExitAction(gmk.GimmickID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanCall()) return;

        RpcTriggerEnterAction(other);

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if (gmk) RpcTriggerEnterAction(gmk.GimmickID);
    }


    private void OnTriggerExit(Collider other)
    {
        if (!CanCall()) return;

        RpcTriggerExitAction(other);

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if (gmk) RpcTriggerExitAction(gmk.GimmickID);
    }

    public void OnPointerHit(Collider rayShooter)
    {
        if (!CanCall()) return;

        var gmk = rayShooter.gameObject.GetComponent<GimmickBase>();
        if (gmk) RpcPointerHitAction(gmk.GimmickID);
    }

    bool CanCall()
    {
        return (!isCallingWithServer || isServer);
    }

    public void DestroyThisObject()
    {
        NetworkServer.Destroy(this.gameObject);
    }
}
