using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using RootMotion.FinalIK;

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


	public void Start()
	{
        StartCoroutine(SetIKCorotuine());
	}

    void SetIKTarget()
    {

        // 0,1
        int playerNum = PlayerManager.GetPlayerNumber() + 1;

        // 1,2
        int[] nums = new int[] { 1, 2 };
        if (!isLocalPlayer)
            playerNum = nums.Where(num => num != playerNum).First();

        transform.parent = GameObject.Find("VRChatCharaPos" + (playerNum)).FindFirstChildByName("VRChatChara").transform;


        var vrIK = GameObject.Find("VRChatCharaPos" + (playerNum)).
            GetComponentInChildren<VRIK>();
        Debug.Log("playerNum : " + playerNum + " Init");

        var targetObj = gameObject.FindFirstChildByName("IKTarget");

        switch (setObjectType)
        {
            case TrackType.RightHand:
                vrIK.solver.rightArm.target = targetObj.transform;
                break;
            case TrackType.LeftHand:
                vrIK.solver.leftArm.target = targetObj.transform;
                //testIKcontrol.rightHandObj = this.transform;
                break;
            case TrackType.RightFoot:
                //vrIK.rightFootObj = this.transform;
                break;
            case TrackType.LeftFoot:
                //vrIK.leftFootObj = this.transform;
                break;
            case TrackType.EyeGazePoint:
                vrIK.solver.spine.headTarget = targetObj.transform;
                //vrIK.lookObj = this.transform;
                break;
        }

        Debug.Log(netId + " " + gameObject.name + "Init");
    }

    IEnumerator SetIKCorotuine()
    {
        yield return new WaitUntil(() => PlayerManager.LocalPlayer != null);

        SetIKTarget();
    }
}
