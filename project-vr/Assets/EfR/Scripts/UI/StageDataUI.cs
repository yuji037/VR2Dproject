using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageDataUI : MonoBehaviour {
    [SerializeField]
    Text stageDescription;

    [SerializeField]
    Text stageLevel;

    [SerializeField]
    Image stageImage;

    public void SetStageData(string description,int level,Sprite image)
    {
        stageDescription.text = description;
        stageLevel.text = "難易度 :"+level;
        stageImage.sprite = image;
    }

}
