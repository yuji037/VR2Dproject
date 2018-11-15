using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// "ゲーム世界にいる"プレイヤーの情報
public class PlayerStatus : NetworkBehaviour {

    public int PlayerNum { get; private set; }

    private void Awake()
    {
        PlayerNum = PlayerManager.GetNewPlayerNum();
        gameObject.name = "Player" + PlayerNum.ToString();

        transform.position = GameObject.Find("PlayerPopPos" + PlayerNum.ToString()).transform.position;
        transform.rotation = GameObject.Find("PlayerPopPos" + PlayerNum.ToString()).transform.rotation;
    }

}
