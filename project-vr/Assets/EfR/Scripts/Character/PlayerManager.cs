using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class PlayerManager {

    public static readonly string[] PlayerNames =
    {
        "Player1",
        "Player2"
    };

    public static GameObject LocalPlayer { get; private set; }

    public static GameObject[] Players { get; private set; }

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

    public static void SetLocalPlayer(GameObject localPlayer)
    {
        LocalPlayer = localPlayer;
    }

    public static void SetPlayers()
    {
        Players = new GameObject[2];
        Players[0] = GameObject.Find(PlayerNames[0]);
        Players[1] = GameObject.Find(PlayerNames[1]);
    }
}
