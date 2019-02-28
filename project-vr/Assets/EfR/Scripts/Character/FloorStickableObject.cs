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
        var origin = transform.position;
        origin.y -= halfExtents.y*0.52f;
        RaycastHit hit;
        if (Physics.Raycast(
            transform.position + new Vector3(0, 0.1f, 0),
            Vector3.down,
            out hit,
            halfExtents.y)&&
            hit.collider.gameObject.tag == "LaserPointerFloorCreate")
        {
            Debug.Log(hit.transform);
            stickTarget = hit.transform;
            preTargetPos = stickTarget.transform.position;
        }
        else
        {
            stickTarget = null;
        }
        if (!IsRigidBodyMove()&&
            stickTarget)
        {
            var move = stickTarget.transform.position - preTargetPos;
            transform.Translate(move,Space.World);
            preTargetPos = stickTarget.transform.position;
            Debug.Log("TR"+move);
        }
    }

    bool IsRigidBodyMove()
    {
        return m_RigidBody &&
            !m_RigidBody.isKinematic;
    }

    private void FixedUpdate()
    {
        if (IsRigidBodyMove()&&
            stickTarget)
        {
            var move=stickTarget.transform.position-preTargetPos;
            m_RigidBody.MovePosition(transform.position+move);
            preTargetPos = stickTarget.transform.position;
            Debug.Log("RI"+move);
        }
    }
}
