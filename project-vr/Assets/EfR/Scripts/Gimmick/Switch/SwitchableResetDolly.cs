using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableResetDolly : MonoBehaviour,ISwitchableObject{
    public void OffAction()
    {
        GetComponent<DollyMoveObject>().autoMove = false;
    }

    public void OnAction()
    {
        GetComponent<DollyMoveObject>().autoMove=true;
    }
}
