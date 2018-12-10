using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPointStorage : MonoBehaviour {

    public Transform CamPosFPS;
    public Transform CamPosTPS;
    public Transform CamPos_2D;
    public Transform CamPosFixed;

    public Transform GetCamPos(PlayerMove.MoveType moveType)
    {
        // 2DだけプレイヤーのプレハブにないのでFindで取得
        // Fixedの時はステージの設定次第と思われるのでFindで取得
        switch ( moveType )
        {
            case PlayerMove.MoveType.FPS:
                return CamPosFPS;

            case PlayerMove.MoveType.TPS:
                return CamPosTPS;

            case PlayerMove.MoveType.FIXED:
                if ( !CamPosFixed )
                    CamPosFixed = GameObject.Find("CamPosFixed").transform;
                return CamPosFixed;

            case PlayerMove.MoveType._2D:
            default:
                if ( !CamPos_2D )
                    CamPos_2D = GameObject.Find("CamPos_2D").transform;
                return CamPos_2D;
        }
    }
}
