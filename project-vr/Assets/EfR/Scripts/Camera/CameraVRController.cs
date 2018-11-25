﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVRController : MonoBehaviour {

    PlayerMove playerMove;
    PlayerStatus playerStatus;
    GameObject targetObject;
    Vector3 targetPosition;

    Transform camPosParent;

    [SerializeField]
    Transform rightHandRig;

    [SerializeField]
    float handRotateSensi = 1f;

    [SerializeField]
    float cameraSensitivity = 10f;

    VRObjectManager vrObjectManager;

    public bool userControllable = true;

    [SerializeField]
    Camera centerCam;

    private Vector3 camChangeDis;
    private Coroutine changeCamera;

    // Use this for initialization
    void Start()
    {

        vrObjectManager = VRObjectManager.GetInstance();

        GameObject oRHAnchor = null;
        switch ( vrObjectManager.DeviceType )
        {
            case VRDeviceType.OCULUS:
                oRHAnchor = GameObject.Find("RightHandAnchor");
                break;
            case VRDeviceType.HTC_VIVE:
                oRHAnchor = GameObject.Find("Controller (right)");
                break;
        }

        if ( oRHAnchor )
        {
            rightHandRig.parent = oRHAnchor.transform;
            rightHandRig.localPosition = Vector3.zero;
            rightHandRig.localEulerAngles = Vector3.zero;
        }

    }

    // プレイヤースポーン後に呼ぶ
    public void Init()
    {

        var oPlayer = PlayerManager.LocalPlayer;
        targetObject = oPlayer;
        playerMove = oPlayer.GetComponent<PlayerMove>();
        playerStatus = oPlayer.GetComponent<PlayerStatus>();

		// カメラを初期位置にセット
		if ( playerMove.moveType != PlayerMove.MoveType._2D )
        {
            var defaultCamPos = playerMove.GetComponent<ViewPointStorage>().GetCamPos(playerMove.moveType);
            transform.position = defaultCamPos.position;

            targetPosition = targetObject.transform.position;
        }

        camPosParent = GameObject.Find("CamPosParent").transform;
        camPosParent.transform.rotation = Quaternion.identity;
        camChangeDis = targetObject.GetComponent<ViewPointStorage>().GetCamPos(PlayerMove.MoveType.TPS).position
            - targetObject.GetComponent<ViewPointStorage>().GetCamPos(PlayerMove.MoveType.FPS).position;

    }

    // Update is called once per frame
    void Update()
    {

        // 非VRなら右手がマウスについてくる
        if ( vrObjectManager.DeviceType == VRDeviceType.NO_DEVICE )
        {
            Vector3 mousePos        = Input.mousePosition;
            mousePos.z              = handRotateSensi;
            Vector3 lookVec         = centerCam.ScreenToWorldPoint(mousePos) - rightHandRig.position;
            rightHandRig.rotation   = Quaternion.LookRotation(lookVec);
        }


        if ( targetObject != null && playerMove.moveType!=PlayerMove.MoveType._2D )
        {
            Vector3 move            = targetObject.transform.position - targetPosition;
            transform.position      += move;
            targetPosition          = targetObject.transform.position;
            //transform.position = targetObject.transform.position;
        }


        // TPS視点ならマウスの中ボタンでカメラをプレイヤーの周囲を回転
        if (    playerMove 
            &&  playerMove.moveType == PlayerMove.MoveType.TPS 
            &&  Input.GetMouseButton(2))
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = -Input.GetAxis("Mouse Y");

            transform.RotateAround(camPosParent.position, Vector3.up,       mouseInputX * cameraSensitivity);
            transform.RotateAround(camPosParent.position, transform.right,  mouseInputY * cameraSensitivity);
        }

        // FPS視点時、GimmickCubeを押していたら視点をTPSに変更する
        RaycastHit hit;
        if (   playerMove
            && playerMove.moveType == PlayerMove.MoveType.FPS
            && Physics.Raycast(targetObject.transform.position, playerMove.GetVelocity(), out hit, 2.0f)
            && hit.transform.tag == "GimmickCube"
            && changeCamera == null)
        {
            playerMove.SwitchMoveType(PlayerMove.MoveType.TPS);
            changeCamera = StartCoroutine(ChangeCamTPS());
            playerStatus.RendererSwitchForPlayerMoveType(PlayerMove.MoveType.TPS);    
        }

        // TPS視点時、GimmickCubeを押していなかったら視点をFPSに変更する
        if ((   playerMove
            && playerMove.moveType == PlayerMove.MoveType.TPS
            && !Physics.Raycast(targetObject.transform.position,playerMove.GetVelocity(), 2.0f)
            && changeCamera == null)
            || playerMove
            && playerMove.moveType == PlayerMove.MoveType.TPS
            && (Physics.Raycast(targetObject.transform.position, playerMove.GetVelocity(),out hit, 2.0f)
            && hit.transform.tag != "GimmickCube"
            && changeCamera == null))
        {
            playerMove.SwitchMoveType(PlayerMove.MoveType.FPS);
            changeCamera = StartCoroutine(ChangeCamFPS());
            playerStatus.RendererSwitchForPlayerMoveType(PlayerMove.MoveType.FPS);
        }
    }

    private IEnumerator ChangeCamTPS()
    {
        for (float f = 0; f < 1.0f; f += Time.deltaTime)
        {
            transform.position += camChangeDis * Time.deltaTime;
            yield return null;
        }
        changeCamera = null;
    }

    private IEnumerator ChangeCamFPS()
    {
        for (float f = 0; f < 1.0f; f += Time.deltaTime)
        {
            transform.position -= camChangeDis * Time.deltaTime;
            yield return null;
        }
        changeCamera = null;
    }
}