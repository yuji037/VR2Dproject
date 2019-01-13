using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSelectStage : SwitchActionBase {
    [SerializeField]
    SelectStageMenu SelectStageMenu;
    public override void OnAction()
    {
        SelectStageMenu.GoToSelectStage();
    }
}
