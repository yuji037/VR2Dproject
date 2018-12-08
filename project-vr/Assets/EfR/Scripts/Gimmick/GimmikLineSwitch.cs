﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmikLineSwitch : GimmickBase {
    [SerializeField]
    SwitchAction switchAction;

    //このLineがPowerOnになるとアクションを起こす
    [SerializeField]
    TransmissionLine SwitchTriggerLine;

    bool preLinePower=false;
    private void Update()
    {
        if (SwitchTriggerLine.IsPowerOn!=preLinePower)
        {
            if (SwitchTriggerLine.IsPowerOn)
            {
                switchAction.OnAction();
            }
            else
            {
                switchAction.OffAction();
            }
        }
        preLinePower = SwitchTriggerLine.IsPowerOn;
    }
}