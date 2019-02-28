using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "EFR/StageData", order = 0)]
public class StageData : ScriptableObject {
    [SerializeField]
    string stageSceneName;
    public string StageSceneName
    {
        get { return stageSceneName; }
    }

    [SerializeField]
    int stageLevel;
    public int StageLevel
    {
        get { return stageLevel; }
    }

    [SerializeField]
    string stageDescription;
    public string StageDescription
    {
        get { return stageDescription; }
    }

    [SerializeField]
    Sprite stageImage;
    public Sprite StageImage
    {
        get { return stageImage;}
    }

	[SerializeField]
	int stageDefaultPlayerLife;
	public int StageDefaultPlayerLife
	{
		get { return stageDefaultPlayerLife; }
	}
}
