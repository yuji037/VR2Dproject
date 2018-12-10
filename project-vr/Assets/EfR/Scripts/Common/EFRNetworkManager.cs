using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EFRNetworkManager : NetworkManager {

    bool connected = false;
    bool isHost = false;

    [SerializeField]
    GameObject m_EFRPlayerPrefab;
	[SerializeField]
	GameObject m_HandRPrefab;
	[SerializeField]
	GameObject m_HandLPrefab;

	public class AddPlayerMessage : MessageBase {
        public int playerNum;
    }

    private void OnGUI()
    {
        if ( !connected )
        {

            if ( GUI.Button(new Rect(10, 30, 200, 30), "LAN Host") )
            {
                StartHost();
                isHost = true;
                connected = true;
            }

            networkAddress = GUI.TextField(new Rect(10, 70, 200, 30), networkAddress);
            if ( GUI.Button(new Rect(10, 100, 200, 30), "LAN Client") )
            {
                StartClient();
                isHost = false;
                connected = true;
            }

			if ( GUI.Button(new Rect(10, 200, 200, 30), "LAN Server Only (使用不可)") )
			{
				StartServer(connectionConfig, maxConnections);
				isHost = false;
				connected = true;
			}
		}
    }

    public void SpawnPlayer()
    {
        var message = new AddPlayerMessage();
        message.playerNum = isHost ? 1 : 2;
        if(ClientScene.AddPlayer(ClientScene.readyConnection, (short)message.playerNum, message) == false)
        {
            Debug.LogError("プレイヤー作成失敗 : " + message.playerNum);
            return;
        }
    }

    public bool IsClientSceneReady()
    {
        return ClientScene.ready;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader reader)
    {
        Debug.Log("サーバーにプレイヤー追加要請");

        var message = reader.ReadMessage<AddPlayerMessage>();
        var player = Instantiate(m_EFRPlayerPrefab);
		var handR = Instantiate(m_HandRPrefab);
		var handL = Instantiate(m_HandLPrefab);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		NetworkServer.AddPlayerForConnection(conn, handR, (short)(playerControllerId + 2));
		NetworkServer.AddPlayerForConnection(conn, handL, (short)(playerControllerId + 4));
		Debug.Log("プレイヤー作成成功 : " + message.playerNum);
        player.GetComponent<PlayerStatus>().number = (message.playerNum - 1);
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect(conn);
	}

	public override void OnStartClient(NetworkClient client)
	{
		base.OnStartClient(client);
	}
}
