﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickSwitchPlayerCameras : GimmickBase{
    bool excuted=false;
    private void Start()
    {
        m_acTriggerEnterAction += Excute;
    }
    void Excute(Collider collider)
    {
        Debug.Log("check"+collider.gameObject);
        if (!excuted&&collider.gameObject==PlayerManager.LocalPlayer)
        {
            Debug.Log("Excuted");
			StartCoroutine(EnterVRCoroutine());
            excuted = true;
        }
    }

	IEnumerator EnterVRCoroutine()
	{
		yield return StartCoroutine(TransP1WorldToFixed());

		if ( PlayerManager.OtherPlayer ) PlayerManager.OtherPlayer.GetComponent<PlayerStatus>().CmdSetActive(true);
		RpcChangeP2CameraTargetToLocalPlayer();
	}

	[ClientRpc]
    void RpcChangeP2CameraTargetToLocalPlayer()
    {
        if (!PlayerManager.CheckLocalPlayerNumber(PlayerNumber.Player2)) return;
       
        var cam2Dcon =GameObject.Find(CameraUtility.Camera2DName).GetComponent<Camera2DController>();
        cam2Dcon.ChangeTargetToLocalPlayer();
        cam2Dcon.NoiseActivate(0.5f,1.0f);
        StageSwitchRenderer.GetInstance().SwitchRenderer(2, true);
        StageSwitchRenderer.GetInstance().SwitchRenderer(1, false);
        StartCoroutine(VRCharaHoloController.GetInstance().VRChatCharaFade(0, false));
    }
  
    IEnumerator TransP1WorldToFixed()
    {
		var vrChatCharaPos = VRCharaHoloController.GetInstance().GetChatCharaObject(0).transform.position;
		var effChannel = EffectManager.GetInstance().Play("CharaParticleAttract3", vrChatCharaPos, true, "VRChatCharaPos1", "TV");
		//yield return new WaitForSeconds(2f);
		StartCoroutine(VRCharaHoloController.GetInstance().VRChatCharaFade(0, false));
        //特別な演出を入れる。
        PlayerManager.playerStatus.RpcTransWorld(PlayerMove.MoveType.FIXED, transform.position);

		yield return new WaitForSeconds(0.5f);
		EffectManager.GetInstance().Stop(effChannel, 2f);
		yield return null;
    }
}
