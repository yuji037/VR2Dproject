using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableTVMask : MonoBehaviour, ISwitchableObject
{
    public void OffAction()
    {
        TVSwitch.IsOn = false;
    }

    public void OnAction()
    {
        TVSwitch.IsOn = true;
    }
}
