using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMSetting", menuName = "BGMSetting", order = 0)]
public class BGMSetting : ScriptableObject {

	public List<StageBGMNamePair> stageBGMNameList;
}

[System.Serializable]
public class StageBGMNamePair {
	public string StageName;
	public string BGMName;
}