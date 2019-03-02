using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerRespawner : SingletonMonoBehaviour<PlayerRespawner>
{
    Vector3 LocalPlayerRespawnPoint;
    public bool isRespawning;

    [Header("残機")]
    public int playerLife = 3;


    public void RespawnLocalPlayer()
    {
        //PlayerManager.LocalPlayer.transform.position = LocalPlayerRespawnPoint;
        //PlayerManager.LocalPlayer.GetComponent<PlayerMove>().ResetVelocity();
        if (isRespawning) return;
        if (playerLife > 0)
        {
            RespawnAndResetDolly.GetInstance().ResetDollys();
			GameOverManager.GetInstance().CmdChangeLife(-1);
            StartCoroutine(RespawnCoroutine());
        }
        else
        {
            StartCoroutine(ToGameOverCoroutine());
        }
    }

    public void GameOverDirectly()
    {
        StartCoroutine(ToGameOverCoroutine());
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
		playerStatus.CmdHoloFade(false, 1f);
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
		playerStatus.CmdHoloFade(true, 1f);
		yield return new WaitUntil(() => playerStatus.IsPerforming == false);

		playerMove.canMove = true;

		EffectManager.GetInstance().Stop(effChannel);

        isRespawning = false;
		Debug.Log("Respawn完了");
	}

    IEnumerator ToGameOverCoroutine()
    {
        isRespawning = true;

        // フェードアウト
        //var respawnPlayerNum = PlayerManager.GetPlayerNumber();
        var respawnPlayer = PlayerManager.LocalPlayer;
        var playerStatus = respawnPlayer.GetComponent<PlayerStatus>();
        var playerMove = respawnPlayer.GetComponent<PlayerMove>();
        playerStatus.IsPerforming = true;
        playerStatus.CmdHoloFade(false, 1f);
        //playerMove.ResetAnimatorParam();


        //// エフェクト再生
        //var effChannel = EffectManager.GetInstance().Play("CharaParticleAttractWarp", respawnPlayer.transform.position, true,
        //    ((int)playerStatus.Number).ToString(), null, LocalPlayerRespawnPoint);

        // 動けないように
        PlayerManager.LocalPlayer.GetComponent<PlayerMove>().ResetVelocity();
        playerMove.canMove = false;

        yield return new WaitUntil(() => playerStatus.IsPerforming == false);
        // フェードアウト完了

        GameOverManager.GetInstance().CmdGameOver();

    }
}
