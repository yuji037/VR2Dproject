using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableMoveDollyObject : MonoBehaviour,ISwitchableObject {
    DollyMoveObject dollyFloor;
    bool isPushing=false;

    [SerializeField]
    bool autoReverse=true;

    private void Start()
    {
        dollyFloor=GetComponent<DollyMoveObject>();
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

    public void OnAction()
    {
        isPushing = true;
    }
    public void OffAction()
    {
        isPushing = false;
    }
}
