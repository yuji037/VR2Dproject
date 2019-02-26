using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AuthorityChanger :  NetworkBehaviour{
    [SerializeField]
    PlayerNumber hasAuthorityPlayerNumber;

    bool initialized = false;

    private void Initialize()
    {
        if (initialized) return;
        var netId = GetComponent<NetworkIdentity>();
        if (netId.localPlayerAuthority) PlayerManager.playerStatus.SetAuth(netId);

        initialized= true;
    }

    bool IsReady()
    {
        return PlayerManager.LocalPlayer &&
            PlayerManager.playerStatus &&
            ((PlayerNumber)PlayerManager.GetPlayerNumber() == hasAuthorityPlayerNumber);
    }
    // Use this for initialization
    void Update() {
        if (IsReady())
        {
            Initialize();
        } 	
	}
	
}
