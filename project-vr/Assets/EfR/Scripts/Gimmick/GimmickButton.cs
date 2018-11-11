using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickButton : GimmickBase {

    public enum Type
    {
        DOOR,
        ACTIVE,
    }

    [SerializeField]
    Type m_eGimmickType;

    [SerializeField]
    int m_iTriggerGimmickID;

    [SerializeField]
    int m_iActorGimmickID;

    [SerializeField]
    Vector3 m_vPushedDirection  = Vector3.down;

    [SerializeField]
    float m_fAnimationCount = 1.0f;

    private Rigidbody m_rRigidbody;
    private Vector3 m_vMoveDistance;
    private bool m_bIsTriggerOn = false;

    private void Start()
    {
        m_rRigidbody = GetComponent<Rigidbody>();
        m_vMoveDistance = ( Multiplication(m_vPushedDirection, transform.localScale) / m_fAnimationCount ) - ( Multiplication( m_vPushedDirection,Vector3.one ) * 0.1f );
    }

    public virtual void OnTrigger()
    {
        if ( !m_bIsTriggerOn )
        {
            StartCoroutine(PushButton());
            m_bIsTriggerOn = true;
        }
    }

    private IEnumerator PushButton()
    {
        for (float t = 0; t < m_fAnimationCount; t += 0.1f)
        {
            m_rRigidbody.MovePosition(m_rRigidbody.position + (m_vMoveDistance * 0.1f));
            yield return null;
        }

        var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
        if (gimik != null)
        {
            switch ( m_eGimmickType )
            {
                case Type.DOOR:
                    var door = gimik as GimmickDoor;
                    door.Open();
                    break;

                case Type.ACTIVE:
                    gimik.gameObject.SetActive(true);
                    break;
            }
        }
        
    }

    public virtual void OffTrigger()
    {
        if ( m_bIsTriggerOn )
        {
            StartCoroutine(ResetButton());
            m_bIsTriggerOn = false;
        }
    }

    private IEnumerator ResetButton()
    {
        for ( float t = 0; t < m_fAnimationCount; t += 0.1f )
        {
            m_rRigidbody.MovePosition(transform.position +( -m_vMoveDistance * 0.1f ));
            yield return null;
        }

        var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
        if (gimik != null)
        {
            switch (m_eGimmickType)
            {
                case Type.DOOR:
                    var door = gimik as GimmickDoor;
                    door.Close();
                    break;

                case Type.ACTIVE:
                    gimik.gameObject.SetActive(false);
                    break;
            }
        }
    }

    Vector3 Multiplication(Vector3 _v1, Vector3 _v2)
    {
        Vector3 vec = Vector3.zero;
        vec.x = _v1.x * _v2.x;
        vec.y = _v1.y * _v2.y;
        vec.z = _v1.z * _v2.z;
        return vec;
    }
}