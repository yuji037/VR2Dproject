using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPointStorage : MonoBehaviour {

	// 2Dだけはリアル世界のTVの前
    public Transform CamPosFPS;
    public Transform CamPosTPS;
    public Transform CamPos_2D;
    public Transform[] CamPosFixed = null;

    public Transform GetCamPos(PlayerMove.MoveType moveType, int sectionNumber = -1)
    {
        // 2Dの時はプレイヤーのプレハブにないのでFindで取得
        // Fixedの時はステージの設定次第と思われるのでFindしてその子を取得
        switch ( moveType )
        {
            case PlayerMove.MoveType.FPS:
                return CamPosFPS;

            case PlayerMove.MoveType.TPS:
                return CamPosTPS;

            case PlayerMove.MoveType.FIXED:
				if ( CamPosFixed == null || CamPosFixed.Length == 0 )
				{
					CamPosFixed = GameObject.Find("CamPosFixed").GetTransformsOrderByNumber("Pos");
				}
                return CamPosFixed[sectionNumber];

            case PlayerMove.MoveType._2D:
            default:
                if ( !CamPos_2D )
                    CamPos_2D = GameObject.Find("CamPos_2D").transform;
                return CamPos_2D;
        }
    }
}
