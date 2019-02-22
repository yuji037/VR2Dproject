using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : SingletonMonoBehaviour<PlayerRespawner>
{
    Vector3 LocalPlayerRespawnPoint;
    bool isRespawning;
    public void RespawnLocalPlayer()
    {
        //PlayerManager.LocalPlayer.transform.position = LocalPlayerRespawnPoint;
        //PlayerManager.LocalPlayer.GetComponent<PlayerMove>().ResetVelocity();
        if (isRespawning) return;
        StartCoroutine(RespawnCoroutine());
    }

    public void SaveLocalPlayerRespawnPoint(Vector3 respawnPoint)
    {
        LocalPlayerRespawnPoint = respawnPoint;
        Debug.Log("Save ローカルプレイヤー");
    }

	IEnumerator RespawnCoroutine()
	{
        isRespawning = true;

		// フェードアウト
		//var respawnPlayerNum = PlayerManager.GetPlayerNumber();
		var respawnPlayer = PlayerManager.LocalPlayer;
		var playerStatus = respawnPlayer.GetComponent<PlayerStatus>();
		var playerMove = respawnPlayer.GetComponent<PlayerMove>();
		playerStatus.IsPerforming = true;
		playerStatus.CmdHoloFade(false);
		playerMove.ResetAnimatorParam();
		

		// エフェクト再生
		var effChannel = EffectManager.GetInstance().Play("CharaParticleAttractWarp", respawnPlayer.transform.position, true,
			((int)playerStatus.Number).ToString(), null, LocalPlayerRespawnPoint);

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

		EffectManager.GetInstance().Stop(effChannel);

        isRespawning = false;
		Debug.Log("Respawn完了");
	}
}
