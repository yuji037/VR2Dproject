using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInformationViewer : SingletonMonoBehaviour<StageInformationViewer>{
    [SerializeField]
    Text stageName;

    [SerializeField]
    GameObject stageInfoRoot;

    public void Play()
    {
        stageInfoRoot.SetActive(true);
        if (GameCoordinator.GetInstance().playingStageData)
        {
            stageName.text=GameCoordinator.GetInstance().playingStageData.StageDescription;
        }
    }

    public void Stop()
    {
        stageInfoRoot.SetActive(false);
    }

}
