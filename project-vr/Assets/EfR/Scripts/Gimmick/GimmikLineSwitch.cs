using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmikLineSwitch : GimmickBase {
    [SerializeField]
    SwitchAction switchAction;

    [SerializeField]
    TransmissionLine transmissionLine;

    bool preLinePower=false;
    private void Update()
    {
        if (transmissionLine.IsPowerOn!=preLinePower)
        {
            if (transmissionLine.IsPowerOn)
            {
                switchAction.OnAction();
            }
            else
            {
                switchAction.OffAction();
            }
        }
        preLinePower = transmissionLine.IsPowerOn;
    }
}
