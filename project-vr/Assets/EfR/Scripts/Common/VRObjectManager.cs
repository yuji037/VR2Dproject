using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public enum VRDeviceType {
    NO_DEVICE,
    OCULUS,
    HTC_VIVE,
}

public class VRObjectManager : SingletonMonoBehaviour<VRObjectManager> {


    [SerializeField]
            GameObject[]    m_prefVRCams;

    public  VRDeviceType    DeviceType { get; private set; }

    public  GameObject      VRCamObject { get; private set; }

	[SerializeField]
			GameObject		m_prefVRHand;


    public  void            SetDeviceType(VRDeviceType deviceType)
    {
        this.DeviceType = deviceType;
    }
    
    public void SpawnVRCamObject()
    {
        var trParent = GameObject.Find("VRCamParent").transform;
        VRCamObject = Instantiate(m_prefVRCams[(int)DeviceType]);

        VRCamObject.transform.parent = trParent;
    }

    NetworkIdentity[] handObjIDs;
    
    public void OnNetworkConnected()
    {
        // 2Pからも1Pの手が見えるように、ネットワーク対応
        var handObjTransforms = VRCamObject.GetComponentsInChildren<Transform>()
								.Where(tr => tr.gameObject.name.Contains("HandRig")).ToArray();

        handObjIDs = new NetworkIdentity[handObjTransforms.Length];
        foreach ( var obj in handObjTransforms )
        {
			//var _parent = obj.transform.parent;
			//obj.transform.parent = null;
			//NetworkServer.Spawn(obj.gameObject);
			//obj.transform.parent = _parent;

			// ※サーバーでスポーンするオブジェクトの親が非アクティブだと
			// 上手くスポーンしないらしい？
			var trackHand = Instantiate(m_prefVRHand);
			//trackHand.transform.parent = GameObject.Find("VRCamParent").transform;
			trackHand.transform.position = obj.transform.position;
			trackHand.transform.rotation = obj.transform.rotation;
			trackHand.GetComponent<TrackingTransform>().trackTransform = obj.transform;
			Debug.Log("OnNetworkConnected");
			NetworkServer.SpawnWithClientAuthority(trackHand, PlayerManager.LocalPlayer);
			//NetworkServer.Spawn(trackHand);
		}

        for(int i = 0; i < handObjTransforms.Length; ++i )
        {
            handObjIDs[i] = handObjTransforms[i].GetComponent<NetworkIdentity>();
        }
    }

    private void Update()
    {
        //foreach ( var ni in handObjIDs )
        //    Debug.Log(ni.netId);
    }

    // プレイヤースポーン後に呼ぶ
    public void InitVRCamObject()
    {
        var oPlayer = PlayerManager.LocalPlayer;
        var plMove = oPlayer.GetComponent<PlayerMove>();

        // カメラのTransform設定
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

        var vca = VRCamObject.GetComponent<VRCameraAdjuster>();
        vca.Init();
    }

    public GameObject GetBaseCameraObject()
    {
        switch ( DeviceType )
        {
            case VRDeviceType.NO_DEVICE:
            case VRDeviceType.OCULUS:
                var oculusCam = VRCamObject.GetComponentsInChildren<Camera>()
                    .Where(cam => cam.gameObject.name == "CenterEyeAnchor").ToArray();
                return oculusCam[0].gameObject;
            case VRDeviceType.HTC_VIVE:
            default:
                var htcCam = VRCamObject.GetComponentsInChildren<Camera>()
                    .Where(cam => cam.gameObject.name == "Camera").ToArray();
                return htcCam[0].gameObject;
        }
    }
}
