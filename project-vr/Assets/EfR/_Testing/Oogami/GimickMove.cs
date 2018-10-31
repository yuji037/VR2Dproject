using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickMove : MonoBehaviour {
    Rigidbody rigidbody;
    [SerializeField] Vector3 initImpulse; 
    // Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(initImpulse, ForceMode.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
