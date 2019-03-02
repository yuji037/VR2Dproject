using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class RespawnAndResetDolly : MonoBehaviour{
    [SerializeField]
    List<int>[] dollyIDsEachPlayer;

    List<DollyMoveObject> dollyMoveObjects;
    static RespawnAndResetDolly instance;
    public static RespawnAndResetDolly GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    private void Start()
    {
        int playerNum = PlayerManager.GetPlayerNumber();
        foreach (var i in dollyIDsEachPlayer[playerNum])
        {
           dollyMoveObjects.Add((GimmickManager.GetActor(i) as MonoBehaviour).GetComponent<DollyMoveObject>());
        }
    }
    public void ResetDollys()
    {
        foreach (var i in dollyMoveObjects)
        {
            i.currentPathValue = 0f;
        }
    }
}
