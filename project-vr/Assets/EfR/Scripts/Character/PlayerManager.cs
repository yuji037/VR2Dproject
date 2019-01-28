using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
  public enum PlayerNumber
    {
        Player1 = 0,
        Player2 = 1,
    }
// MonoBehaviourなのはlocalPlayerをインスペクターで見るため
public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
  
    const string PlayerPrefixName = "Player_";

    [SerializeField]
    GameObject localPlayer;

    static PlayerMove m_playerMove;
    public static PlayerMove playerMove
    {
        get {
            if (!m_playerMove) m_playerMove = LocalPlayer.GetComponent<PlayerMove>();
            return m_playerMove;
        }
        set
        {
            m_playerMove = value;
        }
    }

    static PlayerStatus m_playerStatus;
    public static PlayerStatus playerStatus
    {
        get
        {
            if (!m_playerStatus) m_playerStatus = LocalPlayer.GetComponent<PlayerStatus>();
            return m_playerStatus;
        }
        set
        {
            m_playerStatus = value;
        }
    }
    // このPC端末のプレイヤー
    public static GameObject LocalPlayer { get { return GetInstance().localPlayer; } }

	// 他のPC端末のプレイヤー（1人プレイの時はnull）
	public static GameObject OtherPlayer
	{
		get
		{
			GameObject otherPlayer = Players.Where(obj => obj != LocalPlayer).FirstOrDefault();
			return otherPlayer;
		}
	}
	public static int GetPlayerNumber()
    {
        return (int)LocalPlayer.GetComponent<PlayerStatus>().Number;
    }
    // 接続中の全てのプレイヤー
    public static GameObject[] Players=new GameObject[2];
  
    public static bool CheckLocalPlayerNumber(PlayerNumber playerNumber)
    {
        return (Players[(int)playerNumber] == LocalPlayer);
    }
    // プレイヤー座標が近い順に並べて取得
    public static GameObject[] GetNearPlayers(Vector3 original)
    {
        var nearPlayers = Players.OrderBy(obj => (obj.transform.position - original).sqrMagnitude).ToArray();

        foreach (var pl in nearPlayers)
        {
            Debug.Log(pl.name);
        }

        return nearPlayers;
    }

    public static string GetPlayerName(short playerControllerId)
    {
        return PlayerPrefixName + (playerControllerId + 1).ToString("D2");
    }

    public static void SetLocalPlayer(GameObject _localPlayer)
    {
        GetInstance().localPlayer = _localPlayer;
    }

}
