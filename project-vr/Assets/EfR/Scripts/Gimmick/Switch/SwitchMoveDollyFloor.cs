﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMoveDollyFloor : SwitchActionBase {
    [SerializeField]
    DollyMoveObject dollyFloor;
    bool isPushing=false;

    private void FixedUpdate()
    {
        if (isPushing)
        {
            dollyFloor.Move(1.0f);
        }
        else
        {
            dollyFloor.Move(-1.0f);
        }
    }
    public override void OnAction()
    {
        isPushing = true;
    }
    public override void OffAction()
    {
        isPushing = false;
    }
}
