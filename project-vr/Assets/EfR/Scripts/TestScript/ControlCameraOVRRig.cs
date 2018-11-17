using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCameraOVRRig : MonoBehaviour {

    public GameObject targetObject;
    PlayerMove playerMove;
    Vector3 targetPosition;

    [SerializeField]
    Transform rightHandRig;

    [SerializeField]
    float handRotateSensi = 1f;

    [SerializeField]
    float cameraSensitivity = 10f;

    VRObjectManager vrObjectManager;

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
        playerMove = oPlayer.GetComponent<PlayerMove>();

        if(playerMove.moveType != PlayerMove.MoveType._2D )
        {
            targetObject = oPlayer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( vrObjectManager.DeviceType == VRDeviceType.NO_DEVICE )
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = handRotateSensi;
            Vector3 lookVec = Camera.main.ScreenToWorldPoint(mousePos) - rightHandRig.position;
            rightHandRig.rotation = Quaternion.LookRotation(lookVec);
        }

        if ( targetObject )
        {
            transform.position += targetObject.transform.position - targetPosition;
            targetPosition = targetObject.transform.position;
        }

        // TPS視点ならマウスの中ボタンでカメラをプレイヤーの周囲を回転
        if (    playerMove 
            &&  playerMove.moveType == PlayerMove.MoveType.TPS 
            &&  Input.GetMouseButton(2))
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = -Input.GetAxis("Mouse Y");

            transform.RotateAround(targetPosition, Vector3.up, mouseInputX * cameraSensitivity);
            transform.RotateAround(targetPosition, transform.right, mouseInputY * cameraSensitivity);
        }

    }
}
