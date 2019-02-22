using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMoveDollyFloor : SwitchActionBase {
    [SerializeField]
    DollyMoveObject dollyFloor;
    bool isPushing=false;

    [SerializeField]
    bool autoReverse=true;
    public override List<Transform> ActorObjects
    {
        get
        {
            return new List<Transform>(){dollyFloor.transform};
        }
    }

    private void FixedUpdate()
    {
        if (isPushing)
        {
            dollyFloor.Move(1.0f);
        }
        else if(autoReverse)
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
