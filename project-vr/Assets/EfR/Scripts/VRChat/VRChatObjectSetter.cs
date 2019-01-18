using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class VRChatObjectSetter : NetworkBehaviour {

	public enum TrackType {
		RightHand,
		LeftHand,
		RightFoot,
		LeftFoot,
		EyeGazePoint,
	}

	[SerializeField]
	TrackType setObjectType;

	public void Init()
	{
		int playerNum = PlayerManager.GetPlayerNumber() + 1;

		int[] nums = new int[] { 1, 2 };
		if ( !isLocalPlayer )
			playerNum = nums.Where(num => num != playerNum).First();

		var testIKcontrol = GameObject.Find("VRChatCharaPos" + ( playerNum)).
			GetComponentInChildren<TestIKControl>();
		Debug.Log("playerNum : " + playerNum + " Init");

		switch ( setObjectType )
		{
			case TrackType.RightHand:
				testIKcontrol.rightHandObj = this.transform;
				break;
			case TrackType.LeftHand:
				//testIKcontrol.rightHandObj = this.transform;
				break;
			case TrackType.RightFoot:
				testIKcontrol.rightFootObj = this.transform;
				break;
			case TrackType.LeftFoot:
				testIKcontrol.leftFootObj = this.transform;
				break;
			case TrackType.EyeGazePoint:
				testIKcontrol.lookObj = this.transform;
				break;
		}

		Debug.Log(netId +" " + gameObject.name + "Init");
	}
}
