using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoorOpen : SwitchActionBase
{

    [SerializeField, Header("押したときの影響先ギミックID")]

    protected int m_iActorGimmickID;

    public override List<Transform> ActorObjects
    {
        get
        {
            return new List<Transform>() {ActorDoor.transform};
        }
    }

    GimmickDoor _door;
    GimmickDoor ActorDoor
    {
        get
        {
            if (!_door)
            {
                var gimik = GimmickManager.GetGimmick(m_iActorGimmickID);
                _door = gimik as GimmickDoor;
            }
            return _door;
        }
    }

    public override void OnAction()
    {
        Debug.Log("Switch On");
        if(ActorDoor)ActorDoor.Open();
    }

    public override void OffAction()
    {
        Debug.Log("Switch Off");
        if(ActorDoor)ActorDoor.Close();

    }
}
