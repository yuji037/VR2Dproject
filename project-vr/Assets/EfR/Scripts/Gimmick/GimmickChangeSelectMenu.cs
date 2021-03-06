﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//Host側でのみ判定
public class GimmickChangeSelectMenu : GimmickBase
{
    [SerializeField]
    int triggerID = 1;

    static int isAreaInPlayerCount;

    [SerializeField]
    int requiredPlayerCount;

    // Use this for initialization
    public override void OnStartServer()
    {
        base.OnStartServer();
        isAreaInPlayerCount = 0;
        requiredPlayerCount = (PlayerManager.OtherPlayer)?requiredPlayerCount:1;
        m_aTriggerEnterAction += PlayerWithInArea;
        m_aTriggerExitAction  += PlayerWithOutArea;
    }
    void PlayerWithInArea(int id)
    {
        if (id == triggerID)
        {
            isAreaInPlayerCount++;
        }
        if (ContainsPlayersInArea())
        {
            CmdGoToClearSelect();
        }

    }
    void PlayerWithOutArea(int id)
    {
        if (id == triggerID)
        {
            isAreaInPlayerCount--;
        }
    }

    bool ContainsPlayersInArea()
    {
        var count = isAreaInPlayerCount;
        Debug.Log(count);
        return count >= requiredPlayerCount;
    }
    [Command]
    void CmdGoToClearSelect()
    {
        RpcGoToClearSelect();
    }
    [ClientRpc]
    void RpcGoToClearSelect()
    {
        StartCoroutine(ClearPerformanceRoutine());
    }
    IEnumerator ClearPerformanceRoutine()
    {
        PlayerManager.playerMove.canMove = false;
		PlayerManager.playerMove.ResetAnimatorParam();
		var moveType = PlayerManager.playerMove.moveType;
		var soundManager = SoundManager.GetInstance();
		var speakerPos = moveType == PlayerMove.MoveType._2D ? soundManager.TVSpeakerPos.position : soundManager.VRSpeakerPos.position;
		SoundManager.GetInstance().FadeoutBGM();
		
        //アニメーション出来次第
        //PlayerManager.LocalPlayer.GetComponent<PlayerAnimationController>().CmdSetBool("yorokobi",true);
        yield return new WaitForSeconds(1.0f);
		SoundManager.GetInstance().Play("clear", speakerPos, false);
        yield return new WaitForSeconds(1.0f);
		GameOverManager.GetInstance().SetUIActive(false);
        if (PlayerManager.playerMove.moveType==PlayerMove.MoveType.FIXED)
        {
            PlayerManager.playerStatus.CmdTransWorld(PlayerMove.MoveType._2D,transform.position);
            yield return new WaitForSeconds(3.0f);
        }
		SoundManager.GetInstance().PlayBGM("uresii");
        PlayerManager.playerMove.canMove = true;
        PlayerManager.playerMove.isReady= false;
        ClearSelectMenu.instance.CmdSetActive(true);
    }
}
