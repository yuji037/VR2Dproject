using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickSavableObjectRespawn : GimmickBase{

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;

        var savable = other.gameObject.GetComponent<SavableObject>();
        if (!savable) return;
        StageSaver.GetInstance().Respawn(savable);

    }
    
}
