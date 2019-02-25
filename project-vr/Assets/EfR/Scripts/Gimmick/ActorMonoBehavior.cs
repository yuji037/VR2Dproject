using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMonoBehavior : MonoBehaviour,IActor {

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

    void Awake()
    {
        if (m_iGimmickID != -1)
            GimmickManager.GetInstance().Register(this);

    }

}
