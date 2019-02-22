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
	public GameObject remoteTrackTransform;
	[SerializeField]
	public GameObject localTrackTransform;

	// 対象オブジェクトが動いてから追従したいのでFixedUpdate?
	void Update()
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
		var pos = localTrackTransform.transform.position;
		var rot = localTrackTransform.transform.rotation;

		transform.position = pos;
		transform.rotation = rot;

		remoteTrackTransform.transform.localPosition = transform.localPosition;
		remoteTrackTransform.transform.localRotation = transform.localRotation;
	}

	void SetInRemotePlayer()
	{
		transform.localPosition = remoteTrackTransform.transform.position;
		transform.localRotation = remoteTrackTransform.transform.rotation;
	}

	void FindTrackTransform()
	{
		if ( isLocalPlayer )
		{
			var _name = localTrackGameObjectName;
			if ( _name == "Head" )
			{
				localTrackTransform = VRObjectManager.GetInstance().GetBaseCameraObject();
			}
			else
			{
				var obj = VRObjectManager.GetInstance().VRCamObject.FindFirstChildByName(localTrackGameObjectName);
				localTrackTransform = obj;
			}
		}

		remoteTrackTransform = ClientScene.objects.Values.
			Where(ni =>
		   ni.isLocalPlayer == isLocalPlayer &&
		   ni.gameObject.name.StartsWith(remoteTrackGameObjectName)).FirstOrDefault().gameObject;

		Debug.Log("FindTrackTransform");
	}
}
