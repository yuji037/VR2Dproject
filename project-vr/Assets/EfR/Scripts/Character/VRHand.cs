using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class VRHand : NetworkBehaviour {

    [SyncVar]
    public int playerNumber = -1;

	// Use this for initialization
	void Start () {

		//// 0,1
		//int playerNum = PlayerManager.GetPlayerNumber() + 1;

		//// 1,2
		//int[] nums = new int[] { 1, 2 };
		//if ( !isLocalPlayer )
		//	playerNum = nums.Where(num => num != playerNum).First();



	}

    private void Update()
    {
        if (playerNumber == -1 && isLocalPlayer && PlayerManager.LocalPlayer != null)
        {
            playerNumber = PlayerManager.GetPlayerNumber();
        }
    }

}
