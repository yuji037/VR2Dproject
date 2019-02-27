using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class GimmickWorldLinkFlipFlop : GimmickBase {

	public bool isPlayerOn = false;

	[SerializeField]
	public bool deactivateOnTrigger = true;

	[SerializeField]
	GimmickWorldLinkFlipFlop otherGimmick;

	void Start()
	{
        if (otherGimmick)
            otherGimmick.otherGimmick = this;

        m_acTriggerEnterAction += OnTriggrEnter;
		m_acTriggerExitAction += OnTriggrExit;
	}

	void OnTriggrEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Player" )
		{
			var trigrPlayer = collider.gameObject;
			int playerNum = ( trigrPlayer == PlayerManager.LocalPlayer ) ? 0 : 1;
			CmdSetFrag(playerNum, true);
		}
	}

	void OnTriggrExit(Collider collider)
	{
		if ( collider.gameObject.tag == "Player" )
		{
			var trigrPlayer = collider.gameObject;
			int playerNum = ( trigrPlayer == PlayerManager.LocalPlayer ) ? 0 : 1;
			CmdSetFrag(playerNum, false);
		}
	}

	void CheckFlipFlop(int playerNum)
	{
		//Debug.Log("CheckFlipFlop");
		//if (PlayerManager.OtherPlayer == null )
		//{
		//	if ( frags[0] )
		//	{
		//		FlipFlop();
		//	}
		//}
		//else
		//{
		//	if ( frags.Where(fr => fr == false).Any() == false )
		//	{
		//		FlipFlop();
		//	}
		//}
		if ( isPlayerOn && otherGimmick.isPlayerOn)
		{
			FlipFlop(playerNum);
		}
	}

	// サーバーのみで呼ぶべき
	void FlipFlop(int playerNum)
	{
		FlipFlopPlayersWorld(playerNum);
		//ResetFrags();
		CmdSetFrag(playerNum, false);
		otherGimmick.CmdSetFrag(playerNum, false);
        Deactivate();
	}

	public void Deactivate()
	{
		if ( deactivateOnTrigger )
		{
			//gameObject.SetActive(false);
			DestroyThisObject();
		}

		if ( otherGimmick && otherGimmick.deactivateOnTrigger )
		{
			//otherGimmick.gameObject.SetActive(false);
			otherGimmick.DestroyThisObject();
		}
	}
	

	[Command]
	void CmdSetFrag(int playerNum, bool isTrue)
	{
		//frags[fragNum] = isTrue;
		isPlayerOn = isTrue;
        if (isTrue)
            CheckFlipFlop(playerNum);
	}

	//void ResetFrags()
	//{
	//	for ( int i = 0; i < frags.Length; ++i )
	//		frags[i] = false;
	//}

	// 1人プレイなら1人だけ切り替わる
	void FlipFlopPlayersWorld(int num)
	{
        var numbers = new int[] { 0, 1 };
		var hitPlayer = PlayerManager.Players[num];
        int otherNum = numbers.Where(n => n != num).FirstOrDefault();
		var otherPlayer = PlayerManager.Players[otherNum];

		PlayerMove.MoveType triggeredPlayerTransMoveTypeTo;
		triggeredPlayerTransMoveTypeTo = ( hitPlayer.GetComponent<PlayerMove>().moveType == PlayerMove.MoveType.FIXED ) ?
			PlayerMove.MoveType._2D : PlayerMove.MoveType.FIXED;
		hitPlayer.GetComponent<PlayerStatus>().RpcTransWorld(triggeredPlayerTransMoveTypeTo, transform.position);

		if(otherPlayer != null )
		{
			PlayerMove.MoveType otherPlayerTransMoveTypeTo;
			otherPlayerTransMoveTypeTo = ( otherPlayer.GetComponent<PlayerMove>().moveType == PlayerMove.MoveType.FIXED ) ?
				PlayerMove.MoveType._2D : PlayerMove.MoveType.FIXED;
			otherPlayer.GetComponent<PlayerStatus>().RpcTransWorld(otherPlayerTransMoveTypeTo, otherGimmick.transform.position);
		}
	}

	private void OnDrawGizmos()
	{
		if ( otherGimmick )
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, otherGimmick.transform.position);
		}
	}
}
