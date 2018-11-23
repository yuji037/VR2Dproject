using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVRController : MonoBehaviour {

    PlayerMove playerMove;
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

		// カメラを初期位置にセット
		if ( playerMove.moveType != PlayerMove.MoveType._2D )
        {
            var defaultCamPos = playerMove.GetComponent<ViewPointStorage>().GetCamPos(playerMove.moveType);
            transform.position = defaultCamPos.position;

            targetPosition = targetObject.transform.position;
        }

        camPosParent = GameObject.Find("CamPosParent").transform;
        camPosParent.transform.rotation = Quaternion.identity;

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

    }
}
