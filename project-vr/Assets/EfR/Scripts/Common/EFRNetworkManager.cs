using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EFRNetworkManager : NetworkManager {

    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("Start Client");
        base.OnStartClient(client);
        PlayerManager.SetPlayers();
    }
}
