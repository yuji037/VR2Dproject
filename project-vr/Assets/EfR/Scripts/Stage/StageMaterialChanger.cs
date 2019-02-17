using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageMaterialChanger : SingletonMonoBehaviour<StageMaterialChanger> {

    GameObject[] playerStageRootObject=new GameObject[2];

    [SerializeField]
    string[] changeTargerPrefixs;

    [SerializeField]
    Material _2dMat;

    [SerializeField]
    Material FixedMat;

    public void ChangeMaterial(int playerNumber,PlayerMove.MoveType moveType)
    {
        if(!playerStageRootObject[playerNumber]) playerStageRootObject[playerNumber]= GameObject.Find("P" + (playerNumber+1) + "Objects");
        var renderers=playerStageRootObject[playerNumber].GetComponentsInChildren<Renderer>();
        var targetObjects=renderers.Where(x => changeTargerPrefixs.Where(y => x.gameObject.name.Contains(y)).Count() > 0);
        Material changeMat = null;

        switch (moveType)
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
            case PlayerMove.MoveType.FIXED:
                changeMat = FixedMat;
                break;
            case PlayerMove.MoveType._2D:
                changeMat = _2dMat;
                break;
        }

        foreach (var i in targetObjects)
        {
            i.material = changeMat;
        }

        Debug.Log("Changed StageMaterial: p"+playerNumber+moveType);
    }
}
