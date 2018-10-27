using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusInput : MonoBehaviour {

    private OVRRaycaster ray;
    private OVRCameraRig rig;
    private RaycastHit hit;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("A");
        }

        if (OVRInput.Get(OVRInput.Button.Four))
        {
            Debug.Log("Y");
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("OVRInput.RawButton.LIndexTrigger");
        }

        if (OVRInput.Get(OVRInput.RawTouch.LIndexTrigger))
        {
          //  Debug.Log("OVRInput.RawTouch.LIndexTrigger");
        }

        if (OVRInput.Get(OVRInput.RawNearTouch.LIndexTrigger))
        {
            if (Physics.Raycast(rig.leftHandAnchor.transform.position, ray.transform.forward, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
