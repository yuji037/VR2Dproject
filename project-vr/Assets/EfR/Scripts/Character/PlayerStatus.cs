using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// "ゲーム世界にいる"プレイヤーの情報
public class PlayerStatus : NetworkBehaviour {

    public override void OnStartLocalPlayer()
    {
        if ( isLocalPlayer )
        {
            Debug.Log("Set LocalPlayer");
            PlayerManager.SetLocalPlayer(this.gameObject);
            //GameObject.Find("Server").GetComponent<EFRNetworkServer>().RegisterPlayer(this.gameObject);
        }
    }

    private void Start()
    {
        gameObject.name = "Player" + playerControllerId.ToString();
        Debug.Log("MonoBehaviour Start function");
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
}
