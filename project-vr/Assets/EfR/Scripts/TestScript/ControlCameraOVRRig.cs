using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCameraOVRRig : MonoBehaviour {

    public GameObject targetObject;
    Vector3 targetPosition;

    [SerializeField]
    Transform rightHandRig;

    [SerializeField]
    bool isOculusActive = false;

    [SerializeField]
    float handRotateSensi = 1f;

    [SerializeField]
    float cameraSensitivity = 10f;

    public bool isFPS = true;

    // Use this for initialization
    void Start()
    {
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
        if(!targetObject) targetObject = PlayerManager.GetPlayerThisClient();

        transform.position += targetObject.transform.position - targetPosition;
        targetPosition = targetObject.transform.position;

        if ( !isFPS && Input.GetMouseButton(2))
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = -Input.GetAxis("Mouse Y");

            transform.RotateAround(targetPosition, Vector3.up, mouseInputX * cameraSensitivity);
            transform.RotateAround(targetPosition, transform.right, mouseInputY * cameraSensitivity);
        }

        if ( !isOculusActive )
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = handRotateSensi;
            Vector3 lookVec = Camera.main.ScreenToWorldPoint(mousePos) - rightHandRig.position;
            rightHandRig.rotation = Quaternion.LookRotation(lookVec);
        }
    }
}
