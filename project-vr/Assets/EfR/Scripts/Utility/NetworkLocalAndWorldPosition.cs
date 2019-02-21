using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

// VRチャットの手と頭限定。
public class NetworkLocalAndWorldPosition : NetworkBehaviour {

	[SerializeField]
	public string remoteTrackGameObjectName;
	[SerializeField]
	public string localTrackGameObjectName;
	[SerializeField]
	public Transform remoteTrackTransform;
	[SerializeField]
	public Transform localTrackTransform;

	// 対象オブジェクトが動いてから追従したいのでFixedUpdate
	void FixedUpdate()
	{
		if ( isLocalPlayer && (!localTrackTransform || !remoteTrackTransform) )
		{
			FindTrackTransform();
			return;
		}
		if ( !isLocalPlayer && !remoteTrackTransform )
		{
			FindTrackTransform();
			return;
		}

		if ( isLocalPlayer )
		{
			SetInLocalPlayer();
		}
		else
		{
			SetInRemotePlayer();
		}

	}

	void SetInLocalPlayer()
	{
		var pos = localTrackTransform.position;
		var rot = localTrackTransform.rotation;
		transform.position = pos;
		transform.rotation = rot;

		remoteTrackTransform.localPosition = transform.localPosition;
		remoteTrackTransform.localRotation = transform.localRotation;
	}

	void SetInRemotePlayer()
	{
		transform.localPosition = remoteTrackTransform.position;
		transform.localRotation = remoteTrackTransform.rotation;
	}

	void FindTrackTransform()
	{
		if ( isLocalPlayer )
		{
			var _name = localTrackGameObjectName;
			if ( _name == "Head" )
			{
				localTrackTransform = VRObjectManager.GetInstance().GetBaseCameraObject().transform;
			}
			else
				localTrackTransform = GameObject.Find(localTrackGameObjectName).transform;
		}

		remoteTrackTransform = ClientScene.objects.Values.
			Where(ni =>
		   ni.isLocalPlayer == isLocalPlayer &&
		   ni.gameObject.name.StartsWith(remoteTrackGameObjectName)).FirstOrDefault().transform;
	}
}
