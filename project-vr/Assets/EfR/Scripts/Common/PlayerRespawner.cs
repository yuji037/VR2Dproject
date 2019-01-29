using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : SingletonMonoBehaviour<PlayerRespawner>
{
    Vector3 LocalPlayerRespawnPoint;

    public void RespawnLocalPlayer()
    {
        PlayerManager.LocalPlayer.transform.position = LocalPlayerRespawnPoint;
        PlayerManager.LocalPlayer.GetComponent<PlayerMove>().ResetVelocity();
    }

    public void SaveLocalPlayerRespawnPoint(Vector3 respawnPoint)
    {
        LocalPlayerRespawnPoint = respawnPoint;
        Debug.Log("Save ローカルプレイヤー");
    }
}
