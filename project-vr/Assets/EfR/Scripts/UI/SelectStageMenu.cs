using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SelectStageMenu :NetworkBehaviour 
{
    [SerializeField]
    StageDataMaster stageDataMaster;

    [SerializeField]
    Transform cursor;

    [SerializeField]
    Transform[] cursorPositions;

    int selectingStageIndexNumber;

    [SerializeField]
    GameObject selectMenuObject;

    [Command]
    void CmdGoToSelectStage()
    {
        RpcGoToSelectStage();
    }
    [ClientRpc]
    void RpcGoToSelectStage()
    {
        GoToSelectStage();
    }
    public void GoToSelectStage()
    {
        var selectStageData = stageDataMaster.stageDatas[selectingStageIndexNumber];
        GameCoordinator.GetInstance().playingStageData = selectStageData;
        GameCoordinator.GetInstance().ChangeStage(selectStageData.StageSceneName);
		SoundManager.GetInstance().Play("menu_decision");
	}
	bool isReady;
    public bool IsReady
    {
        get { return isReady; }
        set
        {
            if (value)
            {
                RpcShowSelectUI();
            }
            isReady = value;
        }
    }
    [ClientRpc]
    void RpcShowSelectUI()
    {
        selectMenuObject.SetActive(true);
    }
    float stageUIChangeTimer = 0.4f;
    float stageUIChangeInterval = 0.4f;
    private void Update()
    {
        //サーバーのみインプット可能
        if (!isServer||!isReady) return;

        stageUIChangeTimer += Time.deltaTime;
        if (stageUIChangeTimer <= stageUIChangeInterval) return;

        var inputeEnter= Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Two);
        var inputVertical = Input.GetAxisRaw("Vertical");
        var inputUp = (inputVertical > 0.3f);
        var inputDown =  (inputVertical < -0.3f);
        if (inputeEnter)
        {
            CmdGoToSelectStage();
            stageUIChangeTimer = 0f;
        }
        else if (inputUp)
        {
            RpcStageChangeUp();
            stageUIChangeTimer = 0f;
        }
        else if (inputDown)
        {
            RpcStageChangeDown();
            stageUIChangeTimer = 0f;
        }
    }

    [ClientRpc]
    public void RpcStageChangeDown()
    {
        
        selectingStageIndexNumber++;
        if (selectingStageIndexNumber >= stageDataMaster.stageDatas.Count)
        {
            selectingStageIndexNumber= stageDataMaster.stageDatas.Count-1;
        }
        cursor.transform.position = cursorPositions[selectingStageIndexNumber].position;
		SoundManager.GetInstance().Play("menu_cusorMove");
	}

	[ClientRpc]
    public void RpcStageChangeUp()
    {

        selectingStageIndexNumber--;
        if (selectingStageIndexNumber < 0)
        {
            selectingStageIndexNumber = 0;
        }
        cursor.transform.position = cursorPositions[selectingStageIndexNumber].position;
		SoundManager.GetInstance().Play("menu_cusorMove");
	}



}
