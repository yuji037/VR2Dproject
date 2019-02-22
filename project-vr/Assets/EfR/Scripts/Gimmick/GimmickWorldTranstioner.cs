using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickWorldTranstioner : GimmickBase {

    [SerializeField]
    int triggerGimmickID;

    [SerializeField]
    PlayerMove.MoveType transitionMoveType;
    void Start()
    {
        m_aTriggerEnterAction += TransitionWorld;
    }
    void TransitionWorld(int gimmickID)
    {
        if (gimmickID== triggerGimmickID)
        {
            var pm = PlayerManager.LocalPlayer.GetComponent<PlayerMove>();
            //既に移行したいMoveTypeだった場合か移行中の場合return
            if (transitionMoveType == pm.moveType|| ViewSwitchPerformer.GetInstance().IsTranslation) return;
            pm.SwitchMoveType(transitionMoveType);
            pm.canMove = false;
			pm.SetFixedPosition(transform.position);
            ViewSwitchPerformer.GetInstance().SwitchView(transitionMoveType,() =>
			{
				pm.canMove = true;
			});
        }
    }
}
