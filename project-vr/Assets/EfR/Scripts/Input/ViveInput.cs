using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class ViveInput : MonoBehaviour {

    public GameObject prefab;
    public Rigidbody attachPoint;

    [SteamVR_DefaultAction("Interact")]
    public SteamVR_Action_Boolean spawn;

    SteamVR_Behaviour_Pose trackedObj;
    FixedJoint joint;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void FixedUpdate()
    {
        if (joint == null && spawn.GetStateDown(trackedObj.inputSource))
        {
            var go = GameObject.Instantiate(prefab);
            go.transform.position = attachPoint.transform.position;

            joint = go.AddComponent<FixedJoint>();
            joint.connectedBody = attachPoint;
        }
        else if (joint != null && spawn.GetStateUp(trackedObj.inputSource))
        {
            var go = joint.gameObject;
            var rigidbody = go.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            joint = null;
            Object.Destroy(go, 15.0f);

            // We should probably apply the offset between trackedObj.transform.position
            // and device.transform.pos to insert into the physics sim at the correct
            // location, however, we would then want to predict ahead the visual representation
            // by the same amount we are predicting our render poses.

            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
            if (origin != null)
            {
                rigidbody.velocity = origin.TransformVector(trackedObj.GetVelocity());
                rigidbody.angularVelocity = origin.TransformVector(trackedObj.GetAngularVelocity());
            }
            else
            {
                rigidbody.velocity = trackedObj.GetVelocity();
                rigidbody.angularVelocity = trackedObj.GetAngularVelocity();
            }

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
        }
    }
}
