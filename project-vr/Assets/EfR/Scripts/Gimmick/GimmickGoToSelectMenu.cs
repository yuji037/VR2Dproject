using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickGoToSelectMenu : GimmickBase{

    // Use this for initialization
	void Start () {
        
    }


	[ClientRpc]
    void RpcGoToMenu()
    {
        GameCoordinator.GetInstance().ChangeStageSelectMenu();
    }
}
