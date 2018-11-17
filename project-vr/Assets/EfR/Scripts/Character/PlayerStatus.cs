using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// "ゲーム世界にいる"プレイヤーの情報
public class PlayerStatus : NetworkBehaviour {

    public int PlayerNum { get; private set; }

    private void Awake()
    {
        PlayerNum       = PlayerManager.GetNewPlayerNum();
        gameObject.name = "Player" + PlayerNum.ToString();

        SetPlayerPopPosition();

    }

    public override void OnStartLocalPlayer()
    {
        if ( isLocalPlayer )
        {
            Debug.Log("Set LocalPlayer");
            PlayerManager.SetLocalPlayer(this.gameObject);
        }
    }

    void SetPlayerPopPosition()
    {
        var trPopPos = GameObject.Find("PlayerPopPos" + PlayerNum.ToString());

        if ( trPopPos ) {
            transform.position = trPopPos.transform.position;
            transform.rotation = trPopPos.transform.rotation;
        }
        else
        {
            Debug.LogWarning("PlayerPopPos" + PlayerNum.ToString() + "が見つかりません");
        }
    }
}
