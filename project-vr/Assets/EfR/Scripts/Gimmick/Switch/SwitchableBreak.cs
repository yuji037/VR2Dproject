using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchableBreak: MonoBehaviour,ISwitchableObject
{
    public void OffAction()
    {
        
    }

    public void OnAction()
    {
        var breakableObject=GetComponent<BreakableObject>();
        if (breakableObject) breakableObject.Break();
    }

}
