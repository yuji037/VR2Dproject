using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translation : MonoBehaviour {

    [SerializeField]
    Vector3 vec;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(vec * Time.deltaTime);
        
	}
}
