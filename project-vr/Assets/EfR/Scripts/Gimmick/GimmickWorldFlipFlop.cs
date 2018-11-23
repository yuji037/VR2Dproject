using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class GimmickWorldFlipFlop : GimmickBase {

	[SerializeField]
	int triggerGimmickID;

	[SerializeField, Header("触れたプレイヤーの遷移後視点モード")]
	PlayerMove.MoveType triggeredPlayerTransMoveTypeTo;
	[SerializeField, Header("もう一方のプレイヤーの遷移後視点モード")]
	PlayerMove.MoveType otherPlayerTransMoveTypeTo;

	void Start()
	{
		m_aTriggerEnterAction += FlipFlopPlayerWorldView;
	}

	void FlipFlopPlayerWorldView(int otherGimmickID)
	{
		if ( otherGimmickID == triggerGimmickID )
		{
			//var players = ClientScene.objects.Values.Where(ni => ni.gameObject.name.Contains("Player")).ToArray();

			//foreach ( var pl in players )
			//{
			//	if ( pl.gameObject == null) continue;
			//	var ps = pl.gameObject.GetComponent<PlayerStatus>();
			//	if ( !ps ) continue;
			//	Debug.Log(ps.gameObject.name);
			//	ps.RpcFlipFlopPlayerView(
			//		ps.playerControllerId,
			//		triggeredPlayerTransMoveTypeTo,
			//		otherPlayerTransMoveTypeTo);
			//}
			var nearPlayers = PlayerManager.GetNearPlayers(transform.position);
			var trigrPlayer = nearPlayers[0];
			var otherPlayer = nearPlayers[1];

			trigrPlayer.GetComponent<PlayerStatus>().RpcFlipFlopPlayerView(triggeredPlayerTransMoveTypeTo);
			otherPlayer.GetComponent<PlayerStatus>().RpcFlipFlopPlayerView(otherPlayerTransMoveTypeTo);
		}
	}


}
