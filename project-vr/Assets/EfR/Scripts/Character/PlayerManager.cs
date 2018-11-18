using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class PlayerManager {

    //public static readonly string[] PlayerNames =
    //{
    //    "Player1",
    //    "Player2"
    //};

    public static GameObject LocalPlayer { get; private set; }

    public static GameObject[] Players { get; private set; }

    //public static void LocalPlayerInit()
    //{
    //    LocalPlayer.GetComponent<PlayerStatus>().Init();
    //    LocalPlayer.GetComponent<PlayerMove>().Init();
    //}

    //public static int GetNewPlayerNum()
    //{
    //    return GameObject.Find("Server").GetComponent<EFRNetworkServer>().Players.Count;
    //}

    public static void SetLocalPlayer(GameObject localPlayer)
    {
        LocalPlayer = localPlayer;
    }

    public static void SetPlayers()
    {
        Players = new GameObject[2];
        Players[0] = GameObject.Find("Player1");
        Players[1] = GameObject.Find("Player2");
        Debug.Log("Player[0] : " + Players[0]);
        Debug.Log("Player[1] : " + Players[1]);
    }
}
