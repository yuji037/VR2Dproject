using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataMaster", menuName = "EFR/StageDataMaster", order = 1)]
public class StageDataMaster : ScriptableObject {
    public List<StageData> stageDatas;
}
