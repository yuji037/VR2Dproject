using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : SingletonMonoBehaviour<PlayerRespawner>
{
    Vector3 LocalPlayerRespawnPoint;

    public void RespawnLocalPlayer()
    {
		//PlayerManager.LocalPlayer.transform.position = LocalPlayerRespawnPoint;
		//PlayerManager.LocalPlayer.GetComponent<PlayerMove>().ResetVelocity();
		StartCoroutine(RespawnCoroutine());
    }

    public void SaveLocalPlayerRespawnPoint(Vector3 respawnPoint)
    {
        LocalPlayerRespawnPoint = respawnPoint;
        Debug.Log("Save ローカルプレイヤー");
    }

	IEnumerator RespawnCoroutine()
	{
		// フェードアウト
		//var respawnPlayerNum = PlayerManager.GetPlayerNumber();
		var respawnPlayer = PlayerManager.LocalPlayer;
		var playerStatus = respawnPlayer.GetComponent<PlayerStatus>();
		var playerMove = respawnPlayer.GetComponent<PlayerMove>();
		playerStatus.IsPerforming = true;
		playerStatus.CmdHoloFade(false);

		// 動けないように
        PlayerManager.LocalPlayer.GetComponent<PlayerMove>().ResetVelocity();
		playerMove.canMove = false;

		yield return new WaitUntil(() => playerStatus.IsPerforming == false);
		// フェードアウト完了

		// 位置移動
		PlayerManager.LocalPlayer.transform.position = LocalPlayerRespawnPoint;

		// フェードイン
		playerStatus.IsPerforming = true;
		playerStatus.CmdHoloFade(true);
		yield return new WaitUntil(() => playerStatus.IsPerforming == false);

		playerMove.canMove = true;

		Debug.Log("Respawn完了");
	}
}
