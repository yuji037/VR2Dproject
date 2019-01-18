using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EFRNetworkManager : NetworkManager {

    bool connected = false;
    public bool isHost = false;

    [SerializeField]
    GameObject m_EFRPlayerPrefab;
	[SerializeField]
	GameObject m_HandRPrefab;
	[SerializeField]
	GameObject m_HandLPrefab;
	[SerializeField]
	GameObject m_GazePointPrefab;

	public static string[] curretStageName=new string[2] {"",""};

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
		var gazeObj = Instantiate(m_GazePointPrefab);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		NetworkServer.AddPlayerForConnection(conn, handR, (short)(playerControllerId + 2));
        NetworkServer.AddPlayerForConnection(conn, handL, (short)(playerControllerId + 4));
        NetworkServer.AddPlayerForConnection(conn, gazeObj, (short)(playerControllerId + 6));

		Debug.Log("プレイヤー作成成功 : " + message.playerNum);
        player.GetComponent<PlayerStatus>().RpcInit(message.playerNum - 1);

    }
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
        NetworkServer.SetClientReady(conn);
        Debug.Log("ClientSceneChanged");

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
