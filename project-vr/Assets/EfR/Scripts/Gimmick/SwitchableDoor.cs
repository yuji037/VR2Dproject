using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableDoor : GimmickBase,ISwitchableObject {

    [SerializeField]
    Vector3     m_vOpenedDistance = new Vector3(0f,1f,0f);

    [SerializeField]
    float       m_fPerformDuration = 1.0f;

    [SerializeField]
    bool        m_IsDefaultOpened = false;

    Coroutine m_RunningCoroutine = null;
    Vector3 m_vClosedPosition;

    private void Start()
    {
        m_vClosedPosition = transform.position;
        if ( m_IsDefaultOpened )
            transform.position = transform.position + m_vOpenedDistance;
    }

    public void OnAction()
    {
        if ( m_RunningCoroutine != null)
        {
            StopCoroutine(m_RunningCoroutine);
        }
        m_RunningCoroutine = StartCoroutine(MotionCoroutine(m_vClosedPosition + m_vOpenedDistance));
    }

    public void OffAction()
    {
        if ( m_RunningCoroutine != null )
        {
            StopCoroutine(m_RunningCoroutine);
        }
        m_RunningCoroutine = StartCoroutine(MotionCoroutine(m_vClosedPosition));
    }

    public virtual IEnumerator MotionCoroutine(Vector3 endPos)
    {
        Vector3 startPos = transform.position;
        float moveLen = ( endPos - startPos ).magnitude;
        float dur = m_fPerformDuration * moveLen;

        for(float t = 0; t < dur; t += Time.deltaTime )
        {
            transform.position =
                    startPos    * ( dur - t ) / dur
                +   endPos      * t / dur;

            yield return null;
        }
        transform.position = endPos;
    }


}
