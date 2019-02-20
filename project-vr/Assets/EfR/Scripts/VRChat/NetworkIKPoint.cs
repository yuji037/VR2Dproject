using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkIKPoint : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        transform.parent = GameObject.Find("VRChatObjects").transform;
	}
}
