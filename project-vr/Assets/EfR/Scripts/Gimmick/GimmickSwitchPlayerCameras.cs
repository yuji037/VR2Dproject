using System.Collections;
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
        StageSwitchRenderer.GetInstance().SwitchRenderer(2, true);
        StageSwitchRenderer.GetInstance().SwitchRenderer(1, false);
        StartCoroutine(VRCharaHoloController.GetInstance().VRChatCharaFade(0, false));
    }
  
    IEnumerator TransP1WorldToFixed()
    {
		var vrChatCharaPos = VRCharaHoloController.GetInstance().chatCharas[0].transform.position;
		EffectManager.GetInstance().Play("CharaParticleAttract3", vrChatCharaPos, true, null, "TV");
		StartCoroutine(VRCharaHoloController.GetInstance().VRChatCharaFade(0, false));
		//yield return new WaitForSeconds(3f);
        //特別な演出を入れる。
        PlayerManager.playerStatus.RpcTransWorld(PlayerMove.MoveType.FIXED);
		yield return null;
    }
}
