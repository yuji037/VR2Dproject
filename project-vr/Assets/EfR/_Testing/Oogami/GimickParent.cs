using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickParent : MonoBehaviour {

    [SerializeField] GameObject gimickObj;
    

    // Use this for initialization
    void Start () {
        gimickObj.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = new Vector3( Random.Range(randomMin, randomMax), Random.Range(randomMin, randomMax), initZ);
	}
}
