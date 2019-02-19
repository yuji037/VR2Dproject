using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchBreakObject : SwitchActionBase
{
   

    [SerializeField]
    BreakableObject breakTarget;

    public override List<Transform> ActorObjects
    {
        get
        {
            return new List<Transform>() { breakTarget.transform};
        }
    }

    public override void OnAction()
    {
        breakTarget.Break();
    }

}
