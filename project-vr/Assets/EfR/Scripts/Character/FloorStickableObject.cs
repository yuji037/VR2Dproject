using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class FloorStickableObject : MonoBehaviour {
    Vector3 halfExtents;
    NetworkIdentity netId;
    Vector3 prevPos;
    Transform stickObj;
    Rigidbody m_rigidbody;
  
    Transform defaultParent;
    // Use this for initialization
	void Start () {
        halfExtents = transform.localScale * 0.49f;
        defaultParent = transform.parent;
        netId = GetComponent<NetworkIdentity>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
	RaycastHit[] CheckHit()
    {
        var origin = transform.position;
        origin.y += 0.1f;
        return Physics.BoxCastAll(origin, halfExtents, Vector3.down,transform.rotation ,0.5f);
    }
	// Update is called once per frame
	void FixedUpdate() {
        if (netId && !netId.hasAuthority) return;

        if (stickObj)
        {
            var sub = stickObj.transform.position - prevPos;
            if (m_rigidbody.isKinematic)
            {
                transform.Translate(sub,Space.World);
            }
            else
            {
                m_rigidbody.MovePosition(transform.position+sub);
            }

            prevPos = stickObj.transform.position;
        }

        RaycastHit[] hits = CheckHit();
        if (hits.Length<=0)
        {
                stickObj = null;
        }
        else
        {
            foreach (var hit in hits)
            {
                if(hit.collider.gameObject.tag == "LaserPointerFloorCreate")
                {
                    prevPos = hit.transform.position;
                    stickObj = hit.transform;
                    return;
                }
            }
        }
         
    }
}
