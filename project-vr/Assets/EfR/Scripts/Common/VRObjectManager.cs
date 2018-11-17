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

    GameObject m_oCam;

    public void SetDeviceType(VRDeviceType deviceType)
    {
        this.DeviceType = deviceType;
    }
    
    public void SpawnVRCamObject()
    {
        var trParent = GameObject.Find("VRCamParent").transform;
        m_oCam = Instantiate(m_prefVRCams[(int)DeviceType]);

        m_oCam.transform.parent = trParent;
    }

    // プレイヤースポーン後に呼ぶ
    public void InitVRCamObject()
    {
        var oPlayer = PlayerManager.LocalPlayer;
        var plMove = oPlayer.GetComponent<PlayerMove>();

        // カメラ生成
        var camTr = GameObject.Find("CamPos" + plMove.moveType.ToString()).transform;
        m_oCam.transform.position = camTr.position;
        m_oCam.transform.rotation = camTr.rotation;

        //if ( moveType == MoveType._2D )
        //{
        //    // 2Dの場合のみ、カメラはZ軸の-方向から見る配置にする
        //    var distanceVec = this.transform.position - camTr.position;
        //    camRig.transform.position = this.transform.position - new Vector3(0, 0, distanceVec.magnitude);
        //    camRig.transform.rotation = Quaternion.identity;
        //}
        var cc = m_oCam.GetComponent<ControlCameraOVRRig>();
        cc.Init();

    }
}
