using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageMenu : MonoBehaviour
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
            obj.transform.parent = transform;
            stageDataUIs[i] = obj;
        }
        stageDataUIs[selectingStageIndexNumber].SetActive(true);
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //ここでロードしたい
            //StartCoroutine(StageSceneLoader.GetInstance().LoadStageScene(stageDataMaster.stageDatas[selectingStageIndexNumber].StageSceneName));
        }
    }
    public void StageChangeRight()
    {
        stageDataUIs[selectingStageIndexNumber].SetActive(false);

        selectingStageIndexNumber++;
        if (selectingStageIndexNumber >= stageDataMaster.stageDatas.Count)
        {
            selectingStageIndexNumber = 0;
        }

        stageDataUIs[selectingStageIndexNumber].SetActive(true);
    }

    public void StageChangeLeft()
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
