using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// 当たったものをリスポーンさせる
public class GimmickRespawnSavableObject: GimmickBase{

    private void Start()
    {
        m_acTriggerEnterAction += Respawn;
    }

    private void Respawn(Collider other)
    {
        var savable = other.gameObject.GetComponent<SavableObject>();
        if (!savable) return;
        StageSaver.GetInstance().Respawn(savable);

    }
}
