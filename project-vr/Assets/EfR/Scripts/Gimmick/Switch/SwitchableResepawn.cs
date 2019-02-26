using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SavableObject))]
public class SwitchableResepawn : MonoBehaviour, ISwitchableObject
{
    public void OffAction()
    {
    }

    public void OnAction()
    {
        StageSaver.GetInstance().Respawn(GetComponent<SavableObject>());
    }
}
