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

    public void GoToSelectStage()
    {
        GameCoordinator.GetInstance().ChangeStage(stageDataMaster.stageDatas[selectingStageIndexNumber].StageSceneName);
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
