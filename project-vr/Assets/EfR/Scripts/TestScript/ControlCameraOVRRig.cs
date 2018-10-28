using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCameraOVRRig : MonoBehaviour {

    GameObject targetObject;
    Vector3 targetPosition;

    [SerializeField]
    Transform rightHandRig;

    [SerializeField]
    bool isOculusActive = false;

    [SerializeField]
    float handRotateSensi = 1f;

    // Use this for initialization
    void Start()
    {
        targetObject = GameObject.Find("Player");
        targetPosition = targetObject.transform.position;
        if ( isOculusActive )
        {
            var rh = GameObject.Find("RightHandAnchor");
            rightHandRig.parent = rh.transform;
            rightHandRig.localPosition = Vector3.zero;
            rightHandRig.localEulerAngles = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += targetObject.transform.position - targetPosition;
        targetPosition = targetObject.transform.position;

        if ( !isOculusActive )
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = handRotateSensi;
            Vector3 lookVec = Camera.main.ScreenToWorldPoint(mousePos) - rightHandRig.position;
            rightHandRig.rotation = Quaternion.LookRotation(lookVec);
        }
    }
}
