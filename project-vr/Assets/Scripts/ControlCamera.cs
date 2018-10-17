using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamera : MonoBehaviour {
    GameObject targetObject;
    Vector3 targetPosition;

	// Use this for initialization
	void Start () {
        targetObject = GameObject.Find("Player");
        targetPosition = targetObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += targetObject.transform.position - targetPosition;
        targetPosition = targetObject.transform.position;

        if (Input.GetMouseButton(1))
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = -Input.GetAxis("Mouse Y");

            transform.RotateAround(targetPosition, Vector3.up, mouseInputX * Time.deltaTime * 200.0f);
            transform.RotateAround(targetPosition, transform.right, mouseInputY * Time.deltaTime * 200.0f);
        }
	}
}
