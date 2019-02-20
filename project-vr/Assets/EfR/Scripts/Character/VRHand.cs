using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class VRHand : NetworkBehaviour {

	// Use this for initialization
	void Start () {

		// 0,1
		int playerNum = PlayerManager.GetPlayerNumber() + 1;

		// 1,2
		int[] nums = new int[] { 1, 2 };
		if ( !isLocalPlayer )
			playerNum = nums.Where(num => num != playerNum).First();

		transform.parent = GameObject.Find("VRChatCharaPos" + ( playerNum )).FindFirstChildByName("VRChatChara").transform;


		if (VRObjectManager.GetInstance().DeviceType == VRDeviceType.NO_DEVICE && gameObject.name.Contains("Hand"))
		{
			// デバッグ用にマウスで遊べるよう手をスティック状に伸ばす
			var meshRen = GetComponentInChildren<MeshRenderer>(true);
			var boxCol = GetComponent<BoxCollider>();
			if ( boxCol )
			{
				boxCol.center = new Vector3(0, 0, 5);
				boxCol.size = new Vector3(1, 1, 10);
			}
			if ( meshRen )
				meshRen.enabled = true;
		}
	}
	
}
