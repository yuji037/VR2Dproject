using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class FloorStickableObject : MonoBehaviour
{
    Vector3 halfExtents;
    Vector3 prevPos;
    NetworkIdentity netId;

    Transform defaultParent;

    Rigidbody m_RigidBody;

    Transform stickTarget;
    Vector3 preTargetPos;
    // Use this for initialization
    void Start()
    {
        halfExtents = transform.localScale * 0.49f;
        defaultParent = transform.parent;
        netId = GetComponent<NetworkIdentity>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (netId && !netId.hasAuthority) return;

        RaycastHit hit;
        if (Physics.BoxCast(transform.position, halfExtents, Vector3.down, out hit, transform.rotation, 0.1f) &&
            hit.collider.gameObject.tag == "LaserPointerFloorCreate")
        {
            if (m_RigidBody)
            {
                stickTarget = hit.transform;
                preTargetPos = hit.transform.position;
            }
            else
            {
                transform.parent = hit.transform;
            }
        }
        else
        {
            if (m_RigidBody)
            {
                stickTarget = null;
            }
            else
            {
                transform.parent = defaultParent;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_RigidBody&&
            stickTarget)
        {
            var move=stickTarget.transform.position-preTargetPos;
            m_RigidBody.MovePosition(transform.position+move);
            preTargetPos = stickTarget.transform.position;
        }
    }
}
