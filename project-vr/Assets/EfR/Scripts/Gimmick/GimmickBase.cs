using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public abstract class GimmickBase : NetworkBehaviour,IActor
{

    [SerializeField]
    private int m_iGimmickID = -1;
    public int GimmickID
    {
        get { return m_iGimmickID; }
    }
    public int GetID()
    {
        return m_iGimmickID;
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
    public event Action<int> m_aPointerExitAction;
    public event Action<int> m_aPointerHitAction;

    protected Action<Collision> m_acCollisionEnterAction;
    protected Action<Collision> m_acCollisionExitAction;
    protected Action<Collider> m_acTriggerEnterAction;
    protected Action<Collider> m_acTriggerExitAction;

    // サーバーから全クライアントで呼び出す
    // →ということは各クライアントでDoor.Open()などが起こるのでほんとは[Server]だけで起こるのが正しい？
    // それか呼び出し元が[ServerCallback]でServerのみで呼ばれるのでこちらにAttributeは要らない？
    void CollisionEnterAction(int otherGimmickID)    { if(m_aCollisionEnterAction != null)   m_aCollisionEnterAction(otherGimmickID);}
    void CollisionExitAction(int otherGimmickID)     { if(m_aCollisionExitAction!=null)      m_aCollisionExitAction(otherGimmickID); }
    void TriggerEnterAction(int otherGimmickID)      { if(m_aTriggerEnterAction!=null)       m_aTriggerEnterAction(otherGimmickID);  }
    void TriggerExitAction(int otherGimmickID)       { if(m_aTriggerExitAction != null)      m_aTriggerExitAction(otherGimmickID);   }
    void PointerHitAction(int pointerGimmickID)      { if(m_aPointerHitAction!=null)         m_aPointerHitAction(pointerGimmickID);  }
    void PointerExitAction(int pointerGimmickID)     { if(m_aPointerExitAction != null) m_aPointerExitAction(pointerGimmickID); }


    void CollisionEnterAction(Collision other)       { if(m_acCollisionEnterAction!=null)m_acCollisionEnterAction(other);}
    void CollisionExitAction(Collision other)        { if(m_acCollisionExitAction!=null) m_acCollisionExitAction(other); }
    void TriggerEnterAction(Collider other)          { if(m_acTriggerEnterAction!=null)  m_acTriggerEnterAction(other);  }
    void TriggerExitAction(Collider other)           { if(m_acTriggerExitAction!=null)   m_acTriggerExitAction(other);   }

    //サーバー上でのみ呼び出す時true
    protected bool isCallOnlyServer=true;

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
        CollisionEnterAction(collision);

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if (gmk) CollisionEnterAction(gmk.GimmickID);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!CanCall()) return;

        CollisionExitAction(collision);

        var gmk = collision.gameObject.GetComponent<GimmickBase>();
        if (gmk) CollisionExitAction(gmk.GimmickID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanCall()) return;

        TriggerEnterAction(other);

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if (gmk) TriggerEnterAction(gmk.GimmickID);
    }


    private void OnTriggerExit(Collider other)
    {
        if (!CanCall()) return;

        TriggerExitAction(other);

        var gmk = other.gameObject.GetComponent<GimmickBase>();
        if (gmk) TriggerExitAction(gmk.GimmickID);
    }

    public void OnPointerEnter(Collider rayShooter)
    {
        if (!CanCall()) return;

        var gmk = rayShooter.gameObject.GetComponent<GimmickBase>();
        if (gmk) PointerHitAction(gmk.GimmickID);
    }
    public void OnPointerExit(Collider rayShooter)
    {
        if (!CanCall()) return;

        var gmk = rayShooter.gameObject.GetComponent<GimmickBase>();
        if (gmk) PointerExitAction(gmk.GimmickID);
    }

    bool CanCall()
    {
        return (!gameObject || !isCallOnlyServer || isServer);
    }

    public void DestroyThisObject()
    {
        NetworkServer.Destroy(this.gameObject);
    }


}
