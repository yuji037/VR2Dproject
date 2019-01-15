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

    // このPC端末のプレイヤー
    public static GameObject LocalPlayer { get { return GetInstance().localPlayer; } }

    // 他のPC端末のプレイヤー
    public static GameObject OtherPlayer
    {
        get
        {
            GameObject[] otherPlayers = Players.Where(obj => obj != LocalPlayer).ToArray();
            return otherPlayers[0];
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
        return PlayerPrefixName + playerControllerId.ToString("D2");
    }

    public static void SetLocalPlayer(GameObject _localPlayer)
    {
        GetInstance().localPlayer = _localPlayer;
    }

}
