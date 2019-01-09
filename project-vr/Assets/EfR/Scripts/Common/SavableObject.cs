using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavableObject : MonoBehaviour {
    public event System.Action respawnAction;
    private void Start()
    {
        StageSaver.GetInstance().RegisterSavableObject(this);
    }
    public void ExcuteRespawnAction()
    {
        if (respawnAction != null) respawnAction();
    }
}
