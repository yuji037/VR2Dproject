using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPointStorage : MonoBehaviour
{

    // 2Dだけはリアル世界のTVの前
    public Transform CamPosFPS;
    public Transform CamPosTPS;
    public Transform CamPos_2D;
    public CameraVRController CamVRCon = null;

    public Transform GetCamPos(PlayerMove.MoveType moveType)
    {
        // 2Dの時はプレイヤーのプレハブにないのでFindで取得
        // Fixedの時はステージの設定次第と思われるのでFindしてその子を取得
        switch (moveType)
        {
            case PlayerMove.MoveType.FPS:
                return CamPosFPS;

            case PlayerMove.MoveType.TPS:
                return CamPosTPS;

            case PlayerMove.MoveType.FIXED:
                if(!CamVRCon)
                    CamVRCon = GameObject.Find(CameraUtility.CameraVRName).GetComponent<CameraVRController>();
                return CamVRCon.CurrentVCam.transform;

            case PlayerMove.MoveType._2D:
            default:
                if (!CamPos_2D)
                    CamPos_2D = GameObject.Find("CamPos_2D_" + (PlayerManager.GetPlayerNumber() + 1)).transform;
                return CamPos_2D;
        }
    }
}
