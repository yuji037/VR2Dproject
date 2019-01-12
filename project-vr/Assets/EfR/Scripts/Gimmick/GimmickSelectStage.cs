using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickSelectStage : GimmickBase {
    [System.Serializable]
    enum SelectChangeType
    {
        Right,
        Left,
    }
    [SerializeField]
    SelectChangeType select;

    [SerializeField]
    int triggerID = 1;

    [SerializeField]
    SelectStageMenu stageMenu;

    void Start () {
        m_aTriggerEnterAction += ChangeSelectStage;
	}
    private void ChangeSelectStage(int id)
    {
        if (triggerID == id)
        {
            switch (select)
            {
                case SelectChangeType.Right:
                    stageMenu.StageChangeRight();
                    break;
                case SelectChangeType.Left:
                    stageMenu.StageChangeLeft();
                    break;
            }
        }
    }
}
