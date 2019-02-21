using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SelectStageMenu :NetworkBehaviour 
{
    [SerializeField]
    StageDataMaster stageDataMaster;

    [SerializeField]
    GameObject stageDataUIPrefab;

    GameObject[] stageDataUIs;
    int selectingStageIndexNumber;
    private void Start()
    {
        CreateStageDataUI();
    }

    void CreateStageDataUI()
    {
        stageDataUIs = new GameObject[stageDataMaster.stageDatas.Count];
        for (int i = 0; i < stageDataMaster.stageDatas.Count; i++)
        {
            var obj = Instantiate(stageDataUIPrefab,stageDataUIPrefab.transform.parent);
            var sd = stageDataMaster.stageDatas[i];
            obj.GetComponent<StageDataUI>().SetStageData(sd.StageDescription, sd.StageLevel, sd.StageImage);
            stageDataUIs[i] = obj;
        }
        stageDataUIs[selectingStageIndexNumber].SetActive(true);
    }
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
    }
    public bool isReady;
    float stageUIChangeTimer = 0.4f;
    float stageUIChangeInterval = 0.4f;
    private void Update()
    {
        //サーバーのみインプット可能
        if (!isServer||!isReady) return;

        stageUIChangeTimer += Time.deltaTime;
        if (stageUIChangeTimer <= stageUIChangeInterval) return;

        var inputeEnter= Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Two);
        var inputHorizontal = Input.GetAxisRaw("Horizontal");
        var inputRight = (inputHorizontal > 0.3f);
        var inputLeft =  (inputHorizontal < -0.3f);
        if (inputeEnter)
        {
            CmdGoToSelectStage();
            stageUIChangeTimer = 0f;
        }
        else if (inputRight)
        {
            RpcStageChangeRight();
            stageUIChangeTimer = 0f;
        }
        else if (inputLeft)
        {
            RpcStageChangeLeft();
            stageUIChangeTimer = 0f;
        }
    }

    [ClientRpc]
    public void RpcStageChangeRight()
    {
        stageDataUIs[selectingStageIndexNumber].SetActive(false);

        selectingStageIndexNumber++;
        if (selectingStageIndexNumber >= stageDataMaster.stageDatas.Count)
        {
            selectingStageIndexNumber = 0;
        }

        stageDataUIs[selectingStageIndexNumber].SetActive(true);
    }

    [ClientRpc]
    public void RpcStageChangeLeft()
    {
        stageDataUIs[selectingStageIndexNumber].SetActive(false);

        selectingStageIndexNumber--;
        if (selectingStageIndexNumber < 0)
        {
            selectingStageIndexNumber = stageDataMaster.stageDatas.Count - 1;
        }

        stageDataUIs[selectingStageIndexNumber].SetActive(true);
    }



}
