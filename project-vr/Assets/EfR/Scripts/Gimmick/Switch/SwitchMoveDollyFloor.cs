using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMoveDollyFloor : SwitchActionBase {
    [SerializeField]
    DollyFloor dollyFloor;
    bool isPushing=false;

    private void FixedUpdate()
    {
        if (isPushing)
        {
            Debug.Log("Move");
            dollyFloor.Move(1.0f);
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
