using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallableCube : MonoBehaviour {

    private Rigidbody m_rRigidbody;

    // Use this for initialization
    void Start()
    {
        m_rRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if ( Physics.BoxCast(transform.position, Vector3.one * 0.4f, Vector3.down, Quaternion.identity, 0.1f) ) {
            m_rRigidbody.isKinematic = true;
        }
        else { m_rRigidbody.isKinematic = false; }
    }
}
