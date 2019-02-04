using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FloorStickableObject : MonoBehaviour {
    Vector3 halfExtents;
    Vector3 prevPos;

    Transform defaultParent;
    // Use this for initialization
	void Start () {
        halfExtents = transform.localScale * 0.49f;
        defaultParent = transform.parent;
    }
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        if (Physics.BoxCast(transform.position, halfExtents, Vector3.down,out hit, transform.rotation, 0.1f)&&
            hit.collider.gameObject.tag=="LaserPointerFloorCreate")
        {
            Debug.Log("床に着地");
            transform.parent = hit.transform;
        }
        else
        {
            transform.parent = defaultParent;
        }
    }
}
