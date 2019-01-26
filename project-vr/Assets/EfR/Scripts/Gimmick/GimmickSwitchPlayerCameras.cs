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
            if(PlayerManager.OtherPlayer)PlayerManager.OtherPlayer.GetComponent<PlayerStatus>().CmdSetActive(true);
            RpcChangeP2CameraTargetToLocalPlayer();
            TransP1WorldToFixed();
            excuted = true;
        }
    }
   
    [ClientRpc]
    void RpcChangeP2CameraTargetToLocalPlayer()
    {
        if (!PlayerManager.CheckLocalPlayerNumber(PlayerNumber.Player2)) return;
       
        var cam2Dcon =GameObject.Find(CameraUtility.Camera2DName).GetComponent<Camera2DController>();
        cam2Dcon.ChangeTargetToLocalPlayer();
        StageSwitchRenderer.GetInstance().SwitchRenderer(2, true);
        StageSwitchRenderer.GetInstance().SwitchRenderer(1, false);
        StartCoroutine(VRCharaHoloController.GetInstance().P1VRChatCharaFadeOut());
    }
  
    void TransP1WorldToFixed()
    {
        //特別な演出を入れる。
        PlayerManager.playerStatus.RpcTransWorld(PlayerMove.MoveType.FIXED);
    }
}
