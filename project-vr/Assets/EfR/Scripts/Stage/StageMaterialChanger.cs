using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageMaterialChanger : SingletonMonoBehaviour<StageMaterialChanger>
{

    [System.Serializable]
    struct MaterialParam
    {
        public Material _2dMat;
        public Material VRMat;
        public string prefix;
    }

    GameObject[] playerStageRootObject = new GameObject[2];

    [SerializeField]
    MaterialParam[] materialParams;

    public void ChangeMaterial(int playerNumber, PlayerMove.MoveType moveType)
    {
        if (!playerStageRootObject[playerNumber]) playerStageRootObject[playerNumber] = GameObject.Find("P" + (playerNumber + 1) + "Objects");
        var renderers = playerStageRootObject[playerNumber].GetComponentsInChildren<Renderer>();

        foreach (var i in materialParams)
        {
            var targetObjects = renderers.Where(x => i.prefix.Where(y => x.gameObject.name.Contains(y)).Count() > 0);
            Material changeMat = null;

            switch (moveType)
            {
                case PlayerMove.MoveType.FPS:
                case PlayerMove.MoveType.TPS:
                case PlayerMove.MoveType.FIXED:
                    changeMat = i.VRMat;
                    break;
                case PlayerMove.MoveType._2D:
                    changeMat = i._2dMat;
                    break;
            }

            foreach (var t in targetObjects)
            {
                t.material = changeMat;
            }

        }

        Debug.Log("Changed StageMaterial: p" + playerNumber + moveType);
    }
}
