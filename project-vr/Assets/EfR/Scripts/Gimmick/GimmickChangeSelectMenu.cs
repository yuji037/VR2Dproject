using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickChangeSelectMenu : GimmickBase
{
    [SerializeField]
    int triggerID = 1;

    [SerializeField]
    GimmickChangeSelectMenu linkGimmick;

    public int IsAreaInPlayerCount
    {
        get;
        private set;
    }
    // Use this for initialization
    void Start()
    {
        m_aTriggerEnterAction += PlayerWithInArea;
        m_aTriggerExitAction += PlayerWithOutArea;
    }
    void PlayerWithInArea(int id)
    {
        if (id == triggerID)
        {
            IsAreaInPlayerCount++;
        }
        if (ContainsPlayersInArea())
        {
            Debug.Log("gotoMenu");
            RpcGoToMenu();
        }

    }
    void PlayerWithOutArea(int id)
    {
        if (id == triggerID)
        {
            IsAreaInPlayerCount--;
        }
    }

    bool ContainsPlayersInArea()
    {
        var count = IsAreaInPlayerCount;
        if (linkGimmick) count += linkGimmick.IsAreaInPlayerCount;
        Debug.Log(count);
        return count > 1;
    }

    [ClientRpc]
    void RpcGoToMenu()
    {
        Debug.Log("GoToMenu");
        GameCoordinator.GetInstance().ChangeStageSelectMenu();
    }
}
