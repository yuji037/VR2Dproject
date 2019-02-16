using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoorOpen : SwitchActionBase
{

    [SerializeField, Header("押したときの影響先ギミックID")]

    protected int m_iActorGimmickID;
    public int IActorGimmickID
    {
        get { return m_iActorGimmickID; }
    }

    public override void OnAction()
    {
        Debug.Log("Switch On");
        var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
        var door = gimik as GimmickDoor;
        door.Open();
    }

    public override void OffAction()
    {
        Debug.Log("Switch Off");
        var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
        var door = gimik as GimmickDoor;
        door.Close();

    }
}
