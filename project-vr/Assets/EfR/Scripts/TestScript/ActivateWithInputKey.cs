using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWithInputKey : MonoBehaviour {

    [SerializeField]
    KeyCode keyCode;

    [SerializeField]
    GameObject activateObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetKeyDown(keyCode) )
        {
            activateObj.SetActive(true);
        }
	}
}
