using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager> {

	const string PlayerPrefixName = "Player_";

	[SerializeField]
	GameObject localPlayer;

	// このPC端末のプレイヤー
    public static GameObject LocalPlayer { get { return GetInstance().localPlayer; } }

	// 他のPC端末のプレイヤー
	public static GameObject OtherPlayer { get
		{
			GameObject[]	otherPlayers = Players.Where(obj => obj != LocalPlayer).ToArray();
			return			otherPlayers[0];
		} }

	// 接続中の全てのプレイヤー
    public static GameObject[] Players { get
		{
			var playerNIs = ClientScene.objects.Values.Where(ni => 
				ni.gameObject.name.Contains(PlayerPrefixName)).ToArray();

			var players = new GameObject[playerNIs.Length];
			for(int i = 0; i < players.Length; ++i )
			{
				players[i] = playerNIs[i].gameObject;
			}
			return players;
		}
	}

	// プレイヤー座標が近い順に並べて取得
	public static GameObject[] GetNearPlayers(Vector3 original)
	{
		//float		minSqrDistance	= 9999f;
		//GameObject	nearPlayer		= null;

		//foreach(var pl in Players )
		//{
		//	float	_sqrDistance = ( pl.transform.position - original ).sqrMagnitude;

		//	if(		_sqrDistance < minSqrDistance )
		//	{
		//		minSqrDistance	= _sqrDistance;
		//		nearPlayer		= pl;
		//	}
		//}
		//return nearPlayer;
		var nearPlayers = Players.OrderBy(obj => ( obj.transform.position - original ).sqrMagnitude).ToArray();

		foreach(var pl in nearPlayers )
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
