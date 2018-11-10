//======= Copyright (c) Valve Corporation, All rights reserved. ===============
using UnityEngine;
using System.Collections;

namespace Valve.VR.Extras
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class SteamVR_TestThrowInput : MonoBehaviour
    {
        public GameObject prefab;
        public Rigidbody attachPoint;

        [SteamVR_DefaultAction("Interact")]
        public SteamVR_Action_Boolean spawn;
        [SteamVR_DefaultAction("Interact")]
        public SteamVR_Action_Vector2 _Vector2;
        [SteamVR_DefaultAction("Interact")]
        public SteamVR_Action_Vector3 _Vector3;
        [SteamVR_DefaultAction("Interact")]
        public SteamVR_Action_Single _Single;
        [SteamVR_DefaultAction("Interact")]
        public SteamVR_ActionSet Set;
        [SteamVR_DefaultAction("Interact")]
        public SteamVR_Action_Pose _Pose;
        //[SteamVR_DefaultAction("Interact")]
        //public SteamVR_Action_Boolean spawn;
        //[SteamVR_DefaultAction("Interact")]
        //public SteamVR_Action_Vector3 trigger;
        //[SteamVR_DefaultAction("Interact")]
        //public SteamVR_Action_Single trigger2;



        SteamVR_Behaviour_Pose trackedObj;
        FixedJoint joint;

        private void Awake()
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
            //Debug.Log(trigger2.)

            //// HTCVive繋いでない状態だとクラスが消えてエラーになる
            //if (SteamVR_Input._default.inActions.Teleport.GetStateDown(trackedObj.inputSource))
            //{
            //    Debug.Log("Teleport");
            //}
            //if (0 != SteamVR_Input._default.inActions.Squeeze.GetAxis(trackedObj.inputSource))
            //{
            //    Debug.Log(SteamVR_Input._default.inActions.Squeeze.GetAxis(trackedObj.inputSource));
            //}
            //if (Vector3.zero != SteamVR_Input._default.inActions.SkeletonLeftHand.GetAngularVelocity(trackedObj.inputSource))
            //{
            //    Debug.Log(SteamVR_Input._default.inActions.SkeletonLeftHand.GetAngularVelocity(trackedObj.inputSource));
            //}
            //if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(trackedObj.inputSource))
            //{
            //    Debug.Log("GrabPinch");
            //}
        }
    }
}