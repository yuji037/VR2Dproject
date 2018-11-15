using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager {

    public static readonly string[] PlayerNames =
    {
        "Player1",
        "Player2"
    };

    public static int GetNewPlayerNum()
    {
        int playerNum = 1;
        var existPlayer = GameObject.Find("Player" + playerNum);
        while(existPlayer != null )
        {
            playerNum++;
            existPlayer = GameObject.Find("Player" + playerNum);
        }

        return playerNum;
    }

    // このPCで動かしているプレイヤーオブジェクトを返す。
    public static GameObject GetPlayerThisClient()
    {
        if ( Network.peerType == NetworkPeerType.Disconnected )
            return GameObject.Find(PlayerNames[0]);
        
        if ( Network.isServer )
            return GameObject.Find(PlayerNames[0]);
        else
            return GameObject.Find(PlayerNames[1]);
    }
}
