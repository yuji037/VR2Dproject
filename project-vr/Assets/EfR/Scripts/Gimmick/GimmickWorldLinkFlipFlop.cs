using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class GimmickWorldLinkFlipFlop : GimmickBase {

	static bool[] frags = new bool[2];

	[SerializeField]
	public bool deactivateOnTrigger = true;

	[SerializeField]
	GimmickWorldLinkFlipFlop otherGimmick;

	void Start()
	{
		m_acTriggerEnterAction += OnTriggrEnter;
		m_acTriggerExitAction += OnTriggrExit;
	}

	void OnTriggrEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Player" )
		{
			var trigrPlayer = collider.gameObject;
			int fragNum = ( trigrPlayer == PlayerManager.LocalPlayer ) ? 0 : 1;
			CmdSetFrag(fragNum, true);
			GetComponentInChildren<MeshRenderer>().transform.localScale = new Vector3(1, 5, 1);
		}
	}

	void OnTriggrExit(Collider collider)
	{
		if ( collider.gameObject.tag == "Player" )
		{
			var trigrPlayer = collider.gameObject;
			int fragNum = ( trigrPlayer == PlayerManager.LocalPlayer ) ? 0 : 1;
			CmdSetFrag(fragNum, false);
			GetComponentInChildren<MeshRenderer>().transform.localScale = new Vector3(1, 1, 1);
		}
	}

	void CheckFlipFlop()
	{
		Debug.Log("CheckFlipFlop");
		if (PlayerManager.OtherPlayer == null )
		{
			if ( frags[0] )
			{
				FlipFlop();
			}
		}
		else
		{
			if ( frags.Where(fr => fr == false).Any() == false )
			{
				FlipFlop();
			}
		}
	}

	void FlipFlop()
	{
		FlipFlopPlayersWorld();
		ResetFrags();
		Deactivate();
	}

	public void Deactivate()
	{
		if ( deactivateOnTrigger )
			gameObject.SetActive(false);

		if ( otherGimmick && otherGimmick.deactivateOnTrigger)
			otherGimmick.gameObject.SetActive(false);
	}

	[Command]
	void CmdSetFrag(int fragNum, bool isTrue)
	{
		frags[fragNum] = isTrue;
		CheckFlipFlop();
	}

	void ResetFrags()
	{
		for ( int i = 0; i < frags.Length; ++i )
			frags[i] = false;
	}

	// 1人プレイなら1人だけ切り替わる
	void FlipFlopPlayersWorld()
	{
		var localPlayer = PlayerManager.LocalPlayer;
		var otherPlayer = PlayerManager.OtherPlayer;

		PlayerMove.MoveType triggeredPlayerTransMoveTypeTo;
		triggeredPlayerTransMoveTypeTo = ( localPlayer.GetComponent<PlayerMove>().moveType == PlayerMove.MoveType.FIXED ) ?
			PlayerMove.MoveType._2D : PlayerMove.MoveType.FIXED;
		localPlayer.GetComponent<PlayerStatus>().RpcTransWorld(triggeredPlayerTransMoveTypeTo);

		if(otherPlayer != null )
		{
			PlayerMove.MoveType otherPlayerTransMoveTypeTo;
			otherPlayerTransMoveTypeTo = ( otherPlayer.GetComponent<PlayerMove>().moveType == PlayerMove.MoveType.FIXED ) ?
				PlayerMove.MoveType._2D : PlayerMove.MoveType.FIXED;
			otherPlayer.GetComponent<PlayerStatus>().RpcTransWorld(otherPlayerTransMoveTypeTo);
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
