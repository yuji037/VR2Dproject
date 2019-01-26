using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//Host側でのみ判定
public class GimmickChangeSelectMenu : GimmickBase
{
    [SerializeField]
    int triggerID = 1;

    static int isAreaInPlayerCount;

    int requiredPlayerCount;

    // Use this for initialization
    public override void OnStartServer()
    {
        base.OnStartServer();
        isAreaInPlayerCount = 0;
        requiredPlayerCount = (PlayerManager.OtherPlayer)?2:1;
        m_aTriggerEnterAction += PlayerWithInArea;
        m_aTriggerExitAction  += PlayerWithOutArea;
    }
    void PlayerWithInArea(int id)
    {
        if (id == triggerID)
        {
            isAreaInPlayerCount++;
        }
        if (ContainsPlayersInArea())
        {
            Debug.Log("gotoMenu");
            CmdGoToMenu();
        }

    }
    void PlayerWithOutArea(int id)
    {
        if (id == triggerID)
        {
            isAreaInPlayerCount--;
        }
    }

    bool ContainsPlayersInArea()
    {
        var count = isAreaInPlayerCount;
        Debug.Log(count);
        return count >= requiredPlayerCount;
    }
    [Command]
    void CmdGoToMenu()
    {
        RpcGoToMenu();
    }
    [ClientRpc]
    void RpcGoToMenu()
    {
        Debug.Log("GoToMenu");
        GameCoordinator.GetInstance().ChangeStageSelectMenu();
    }
}
