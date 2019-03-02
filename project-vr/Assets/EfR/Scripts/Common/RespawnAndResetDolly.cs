using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class RespawnAndResetDolly : MonoBehaviour{
    [System.Serializable]
    class IDList
    {
        public List<int> ids = new List<int>();
    }
    [SerializeField]
    IDList[] dollyIDsEachPlayer;

    List<DollyMoveObject> dollyMoveObjects=new List<DollyMoveObject>();
    static RespawnAndResetDolly instance;
    public static RespawnAndResetDolly GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    bool initialized = false;
    private void Update()
    {
        if (PlayerManager.LocalPlayer&&!initialized)
        {
            int playerNum = PlayerManager.GetPlayerNumber();
            foreach (var i in dollyIDsEachPlayer[playerNum].ids)
            {
                dollyMoveObjects.Add((GimmickManager.GetActor(i) as MonoBehaviour).GetComponent<DollyMoveObject>());
            }
            initialized = true;
        }
    }
    public void ResetDollys()
    {
        foreach (var i in dollyMoveObjects)
        {
            i.currentPathValue = 0f;
            i.autoMove = false;
            i.Move(0f);
        }
    }
}
