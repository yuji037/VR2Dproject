using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchResetSavableObjects : SwitchActionBase{
    [SerializeField]
    SavableObject[] savableObjects;
    public override void OnAction()
    {
        foreach (var i in savableObjects)
        {
            StageSaver.GetInstance().Respawn(i);
        }
    }
}
