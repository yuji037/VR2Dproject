using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class FloorStickableObject : MonoBehaviour {
    Vector3 halfExtents;
    NetworkIdentity netId;

    Transform defaultParent;
    // Use this for initialization
	void Start () {
        halfExtents = transform.localScale * 0.49f;
        defaultParent = transform.parent;
        netId = GetComponent<NetworkIdentity>();
    }
	
	// Update is called once per frame
	void Update () {
        if (netId && !netId.hasAuthority) return;

        RaycastHit hit;
        if (Physics.BoxCast(transform.position, halfExtents, Vector3.down,out hit, transform.rotation, 0.1f)&&
            hit.collider.gameObject.tag=="LaserPointerFloorCreate")
        {
            transform.SetParent(hit.transform,false);
        }
        else
        {
            transform.SetParent(defaultParent,false);
        }
    }
}
