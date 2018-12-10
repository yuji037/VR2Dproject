using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

// "ゲーム世界にいる"プレイヤーの情報
public class PlayerStatus : NetworkBehaviour {

	[SerializeField]
	GameObject m_prefVRHand;
    [SyncVar]
    public int number;


    public override void OnStartLocalPlayer()
    {
        if ( isLocalPlayer )
        {
            Debug.Log("Set LocalPlayer");
            PlayerManager.SetLocalPlayer(this.gameObject);
		}
	}

    private void Start()
    {
		gameObject.name = PlayerManager.GetPlayerName(playerControllerId);
        PlayerManager.Players[number] = gameObject;
        StageInit();
    }

    public void StageInit()
    {
        SetPlayerPopPosition();
    }

    public void RendererSwitchForPlayerMoveType(PlayerMove.MoveType moveType)
    {
        var renderers = GetComponentsInChildren<Renderer>();

        switch ( moveType )
        {
            case PlayerMove.MoveType.FPS:
                foreach ( var ren in renderers )
                {
                    ren.enabled = false;
                }
                break;

            case PlayerMove.MoveType.TPS:
            case PlayerMove.MoveType._2D:
            case PlayerMove.MoveType.FIXED:
                foreach ( var ren in renderers )
                {
                    ren.enabled = true;
                }
                break;
        }
    }

    void SetPlayerPopPosition()
    {
        if ( !isLocalPlayer ) return;

        var trPopPos = GameObject.Find("PlayerPopPos" + playerControllerId.ToString());

        if ( trPopPos ) {
            transform.position = trPopPos.transform.position;
            transform.rotation = trPopPos.transform.rotation;
        }
        else
        {
            Debug.LogWarning("PlayerPopPos" + playerControllerId.ToString() + "が見つかりません");
        }
    }

	// 他端末のプレイヤーに影響する場合は[ClientRpc]を使う
	// [ClientRpc]：すべてのクライアントの関数をリモートで実行
	[ClientRpc]
	public void RpcTransWorld(PlayerMove.MoveType transMoveTypeTo)
	{
		if ( !hasAuthority ) return;

		var pm = GetComponent<PlayerMove>();
		Debug.Log(pm.gameObject.name + " : " + pm.moveType + " → " + transMoveTypeTo);

		//既に移行したいMoveTypeだった場合か移行中の場合return
		if ( transMoveTypeTo == pm.moveType || ViewSwitchPerformer.GetInstance().IsTranslation ) return;
		pm.SwitchMoveType(transMoveTypeTo);
		pm.canMove = false;
		ViewSwitchPerformer.GetInstance().SwitchView(transMoveTypeTo, () => pm.canMove = true);
	}

}
