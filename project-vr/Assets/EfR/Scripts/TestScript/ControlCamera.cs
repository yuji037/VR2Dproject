using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamera : MonoBehaviour {
    GameObject targetObject;
    Vector3 targetPosition;

    public bool rotate = false;
    public bool is2D = false;

    [SerializeField]
    float cameraSensitivity = 1f;

	// Use this for initialization
	void Start () {
        targetObject = PlayerManager.LocalPlayer;
        targetPosition = targetObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if ( !targetObject ) targetObject = PlayerManager.LocalPlayer;

        Vector3 move = targetObject.transform.position - targetPosition;
        if ( is2D ) move.y = 0;
        transform.position += move;
        targetPosition = targetObject.transform.position;

        if (Input.GetMouseButton(1) && rotate)
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = -Input.GetAxis("Mouse Y");

            transform.RotateAround(targetPosition, Vector3.up, mouseInputX * cameraSensitivity);
            transform.RotateAround(targetPosition, transform.right, mouseInputY * cameraSensitivity);
        }
        if ( Input.GetKeyDown(KeyCode.C) )
        {
            rotate = true;
        }
	}
}
