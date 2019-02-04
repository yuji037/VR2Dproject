using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraVRController : CameraControllerBase
{

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

   public Camera CenterCam
    {
        get { return centerCam; }
    }

    // Use this for initialization
    void Start()
    {

        vrObjectManager = VRObjectManager.GetInstance();

        GameObject oRHAnchor = null;
        switch (vrObjectManager.DeviceType)
        {
            case VRDeviceType.OCULUS:
                oRHAnchor = GameObject.Find("RightHandAnchor");
                break;
            case VRDeviceType.HTC_VIVE:
                oRHAnchor = GameObject.Find("Controller (right)");
                break;
        }

        if (oRHAnchor)
        {
            rightHandRig.parent = oRHAnchor.transform;
            rightHandRig.localPosition = Vector3.zero;
            rightHandRig.localEulerAngles = Vector3.zero;
        }

    }

    // プレイヤースポーン後に呼ぶ
    public void StageInit()
    {

        var oPlayer = PlayerManager.LocalPlayer;
        targetObject = oPlayer;
        playerMove = oPlayer.GetComponent<PlayerMove>();
        playerStatus = oPlayer.GetComponent<PlayerStatus>();

        switch (playerMove.moveType)
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
                // 目標点をプレイヤーに
                targetPosition = targetObject.transform.position;
                break;
            case PlayerMove.MoveType._2D:
                HasCameraAuthority = false;
                var Cam2d = PlayerManager.LocalPlayer.GetComponent<ViewPointStorage>().GetCamPos(PlayerMove.MoveType._2D);
                transform.position = Cam2d.position;
                transform.rotation = Cam2d.rotation;
                break;
            case PlayerMove.MoveType.FIXED:
                HasCameraAuthority = true;
                break;
        }
        Debug.Log("change"+playerMove.moveType);
        camPosParent = PlayerManager.LocalPlayer.FindFirstChildByName("CamPosParent").transform;
        camPosParent.transform.localRotation = Quaternion.identity;

        var vps = targetObject.GetComponent<ViewPointStorage>();

    }

    // 対象オブジェクトが動いてから追従したいのでFixedUpdate
    void FixedUpdate()
    {

        // 非VRなら右手がマウスについてくる
        if (vrObjectManager.DeviceType == VRDeviceType.NO_DEVICE)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = handRotateSensi;
            Vector3 lookVec = centerCam.ScreenToWorldPoint(mousePos) - rightHandRig.position;
            rightHandRig.rotation = Quaternion.LookRotation(lookVec);
        }

        if (playerMove && playerMove.moveType == PlayerMove.MoveType.FIXED)
        {
            return;
        }


        if (targetObject != null && playerMove.moveType != PlayerMove.MoveType._2D)
        {
            Vector3 move = targetObject.transform.position - targetPosition;
            transform.position += move;
            targetPosition = targetObject.transform.position;
            //transform.position = targetObject.transform.position;
        }

        PlayerRotate();

    }

    void PlayerRotate()
    {
        if (playerMove == null) return;

        Vector2 inputRightStick = Vector2.zero;
        switch (vrObjectManager.DeviceType)
        {
            case VRDeviceType.NO_DEVICE:
                if (Input.GetMouseButton(2))
                {
                    inputRightStick = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
                }
                break;
            case VRDeviceType.OCULUS:
                inputRightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                break;
            case VRDeviceType.HTC_VIVE:
                break;
        }

        if (playerMove.moveType == PlayerMove.MoveType.FPS)
        {
            transform.Rotate(new Vector3(0, inputRightStick.x));
        }
        else if (playerMove.moveType == PlayerMove.MoveType.TPS)
        {
            // TPS視点ならマウスの中ボタンでカメラをプレイヤーの周囲を回転
            transform.RotateAround(camPosParent.position, Vector3.up, inputRightStick.x * cameraSensitivity);
            //transform.RotateAround(camPosParent.position, transform.right,  mouseInputY * cameraSensitivity);
        }
    }
}
