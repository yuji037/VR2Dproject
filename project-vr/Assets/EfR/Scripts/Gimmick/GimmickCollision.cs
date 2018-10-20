using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 一応Collision、Triggerどちらにも対応するよう作成
public class GimmickCollision : Gimmick {

    [SerializeField]
    int[] eventTriggerGimmickIDs;

    private void OnCollisionEnter(Collision collision)
    {
        var gimmick = collision.gameObject.GetComponent<Gimmick>();
        if ( !gimmick ) return;

        // 指定IDと合致するか調べる
        bool causedEvent = false;
        foreach(var id in eventTriggerGimmickIDs )
        {
            if(id == gimmick.ID )
            {
                causedEvent = true;
            }
        }
        if ( !causedEvent ) return;

        // 条件クリアしたので、イベント実行
        for(int i = 0; i < sendEventList.Length; ++i )
        {
            GimmickManager.GetInstance().ReceiveMessage(sendEventList[i].gimmickID, sendEventList[i].sendMessage);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        var gimmick = collider.gameObject.GetComponent<Gimmick>();
        if ( !gimmick ) return;

        // 指定IDと合致するか調べる
        bool causedEvent = false;
        foreach ( var id in eventTriggerGimmickIDs )
        {
            if ( id == gimmick.ID )
            {
                causedEvent = true;
            }
        }
        if ( !causedEvent ) return;

        // 条件クリアしたので、イベント実行
        for ( int i = 0; i < sendEventList.Length; ++i )
        {
            GimmickManager.GetInstance().ReceiveMessage(sendEventList[i].gimmickID, sendEventList[i].sendMessage);
        }
    }
}
