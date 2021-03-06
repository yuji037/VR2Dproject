﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PushableObject : MonoBehaviour {

    [SerializeField]
    LayerMask triggerLayerMask = -1;
    [SerializeField]
    float pushStrength = 0.5f;
    [SerializeField]
    LayerMask stoppableLayerMask;
	Vector3 halfExtents;
	Rigidbody rigidBddy;

    NetworkIdentity netId;
	private void Start()
	{
		halfExtents = transform.lossyScale * 0.49f;
        netId = GetComponent<UnityEngine.Networking.NetworkIdentity>();
    }

	private void OnTriggerStay(Collider other)
    {
        if (netId && !netId.hasAuthority) return;

        var otherLayerMask = 1 << other.gameObject.layer;
        // 選択したレイヤー以外のオブジェクトに当たっても押されない
        if ( (otherLayerMask & triggerLayerMask.value) == 0 ) return;

        Vector3 distance = transform.position - other.transform.position;
        float x = Mathf.Abs(distance.x);
        float z = Mathf.Abs(distance.z);

        if ( x > z ) { distance.z = 0; }
        else { distance.x = 0; }
        distance.y = 0;

		// 物体同士が近いほど押される距離を長くする
        distance /= distance.magnitude;
		

        // 押される方向に当たり判定がある物体があったら押されない
        if ( Physics.BoxCast(transform.position, halfExtents, distance.normalized, transform.rotation, 
			distance.magnitude * pushStrength * Time.deltaTime,
            stoppableLayerMask
            ) )
            return;

        transform.position += (distance * pushStrength * Time.deltaTime);
    }
}
