using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
public class GimmickSave : GimmickBase
{
    [SerializeField]
    int[] triggerGimmickIDs;

    [SerializeField]
    Transform respawnPoint;

    private void Start()
    {
        m_acTriggerEnterAction += Save;
    }

    void Save(Collider collider)
    {
        //ギミックIDチェック
        var gb = collider.GetComponent<GimmickBase>();
        if (!gb || !triggerGimmickIDs.Contains(gb.GimmickID)) return;

        //プレイヤーならリスポーンポイントを登録
        var ps = collider.GetComponent<PlayerStatus>();
        if (!ps) return;
        RpcSave((int)ps.Number);
    }
    [ClientRpc]
    void RpcSave(int playerNumber)
    {
        if (PlayerManager.LocalPlayer.GetComponent<PlayerStatus>().Number == (PlayerNumber)playerNumber)
        {
            PlayerRespawner.GetInstance().SaveLocalPlayerRespawnPoint(respawnPoint.position);
        }
    }
    }
