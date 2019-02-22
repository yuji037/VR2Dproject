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

	//[SerializeField, Header("一回触れると消えるかどうか")]
	//bool destroyOnTrigger = true;

	void Start()
	{
		m_aTriggerEnterAction += FlipFlopPlayersWorld;
	}

	// 両プレイヤーの視点を設定に従って切り替える
	// 1人プレイなら1人だけ切り替わる
	void FlipFlopPlayersWorld(int otherGimmickID)
	{
		if ( otherGimmickID == triggerGimmickID )
		{
			var nearPlayers = PlayerManager.GetNearPlayers(transform.position);
			var trigrPlayer = nearPlayers[0];
			var otherPlayer = nearPlayers.Length > 1 ? nearPlayers[1] : null;

			trigrPlayer.GetComponent<PlayerStatus>().RpcTransWorld(triggeredPlayerTransMoveTypeTo, transform.position);
			if ( otherPlayer )
				otherPlayer.GetComponent<PlayerStatus>().RpcTransWorld(otherPlayerTransMoveTypeTo, transform.position);

			//if ( destroyOnTrigger )
			//	DestroyThisObject();
		}
	}


}
