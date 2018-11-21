﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrackingTransform : NetworkBehaviour {

	public string trackGameObjectName;
	public Transform trackTransform;

	// Update is called once per frame
	void Update()
	{
		if ( !isLocalPlayer ) return;
		if ( !trackTransform )
		{
			trackTransform = GameObject.Find(trackGameObjectName).transform;
			return;
		}

		transform.position = trackTransform.position;
		transform.rotation = trackTransform.rotation;
	}
}