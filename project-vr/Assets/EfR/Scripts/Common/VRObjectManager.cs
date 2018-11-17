using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VRDeviceType {
    NO_DEVICE,
    OCULUS,
    HTC_VIVE,
}

public class VRObjectManager : SingletonMonoBehaviour<VRObjectManager> {


    [SerializeField]
    GameObject[] m_prefVRCams;

    public VRDeviceType DeviceType { get; private set; }

    public GameObject VRCamObject { get; private set; }

    public void SetDeviceType(VRDeviceType deviceType)
    {
        this.DeviceType = deviceType;
    }
    
    public void SpawnVRCamObject()
    {
        var trParent = GameObject.Find("VRCamParent").transform;
        VRCamObject = Instantiate(m_prefVRCams[(int)DeviceType]);

        VRCamObject.transform.parent = trParent;
    }

    // プレイヤースポーン後に呼ぶ
    public void InitVRCamObject()
    {
        var oPlayer = PlayerManager.LocalPlayer;
        var plMove = oPlayer.GetComponent<PlayerMove>();

        // カメラ生成
        var camTr = oPlayer.GetComponent<ViewPointStorage>().GetCamPos(plMove.moveType);
        VRCamObject.transform.position = camTr.position;
        VRCamObject.transform.rotation = camTr.rotation;

        //if ( moveType == MoveType._2D )
        //{
        //    // 2Dの場合のみ、カメラはZ軸の-方向から見る配置にする
        //    var distanceVec = this.transform.position - camTr.position;
        //    camRig.transform.position = this.transform.position - new Vector3(0, 0, distanceVec.magnitude);
        //    camRig.transform.rotation = Quaternion.identity;
        //}
        var cc = VRCamObject.GetComponent<CameraVRController>();
        cc.Init();

    }
}
