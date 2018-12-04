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
		// 落下判定
        if ( Physics.BoxCast(transform.position, Vector3.one * 0.4f, Vector3.down, Quaternion.identity, 0.1f) ) {
			// 落下しない
            m_rRigidbody.isKinematic = true;
        }
        else {
			// 落下する
			m_rRigidbody.isKinematic = false;
		}

		//// XZ座標で中途半端な座標にならないように補正
		//var targetXZ = transform.position;
		//targetXZ.x	= Mathf.Round(targetXZ.x);
		//targetXZ.z	= Mathf.Round(targetXZ.z);
	}
}
