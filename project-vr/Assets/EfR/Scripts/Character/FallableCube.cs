using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FallableCube : MonoBehaviour {

    private Rigidbody m_rRigidbody;
	Vector3 halfExtents;

    NetworkIdentity netId;
    // Use this for initialization
    void Start()
    {
        m_rRigidbody = GetComponent<Rigidbody>();
		halfExtents = transform.lossyScale * 0.49f;
        netId = GetComponent<NetworkIdentity>();
    }

    void FixedUpdate()
    {
        if (netId && !netId.hasAuthority) return;
        RaycastHit hit;
		// 落下判定
        if ( Physics.BoxCast(transform.position, halfExtents, Vector3.down, out  hit, transform.rotation, 0.1f)&&
            !hit.collider.isTrigger) {
			// 落下しない
            m_rRigidbody.isKinematic = true;
        }
        else if( !gameObject.isStatic ) {
			// 落下する
			m_rRigidbody.isKinematic = false;
		}

		//// XZ座標で中途半端な座標にならないように補正
		//var targetXZ = transform.position;
		//targetXZ.x	= Mathf.Round(targetXZ.x);
		//targetXZ.z	= Mathf.Round(targetXZ.z);
	}
}
