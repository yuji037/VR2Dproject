using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickDoor : GimmickBase {

    [SerializeField]
    bool m_IsOpened = false;
    
    public virtual void Open()
    {
        if ( !m_IsOpened )
        {
            StartCoroutine(OpenCoroutine());
            m_IsOpened = true;
        }
    }

    public virtual IEnumerator OpenCoroutine()
    {
        Vector3 defaultPos = transform.position;
        for(float t = 0; t < 1; t += Time.deltaTime )
        {
            transform.position = defaultPos + new Vector3(0, t*2f, 0);
            yield return null;
        }
    }

    public virtual void Close()
    {
        if ( m_IsOpened )
        {
            StartCoroutine(CloseCoroutine());
            m_IsOpened = false;
        }
    }

    public virtual IEnumerator CloseCoroutine()
    {
        Vector3 defaultPos = transform.position;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            transform.position = defaultPos - new Vector3(0, t * 2f, 0);
            yield return null;
        }
    }
}
