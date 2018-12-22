﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : SingletonMonoBehaviour<PlayerRespawner>
{
    bool initialized = false;
    Vector3 LocalPlayerRespawnPoint;
    private void Update()
    {
        if (!initialized&&PlayerManager.LocalPlayer)
        {
            SaveLocalPlayerRespawnPoint(PlayerManager.LocalPlayer.transform.position);
            initialized = true;
        }
    }

    public void RespawnLocalPlayer()
    {
        DebugTools.Log("Respawn ローカルプレイヤー");
        PlayerManager.LocalPlayer.transform.position = LocalPlayerRespawnPoint;
        PlayerManager.LocalPlayer.GetComponent<PlayerMove>().ResetVelocity();
    }

    public void SaveLocalPlayerRespawnPoint(Vector3 respawnPoint)
    {
        LocalPlayerRespawnPoint = respawnPoint;
        Debug.Log("Save ローカルプレイヤー");
    }
}