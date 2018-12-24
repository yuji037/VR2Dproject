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

		Debug.Log("VR Camera Spawn");
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

    // プレイヤースポーン後に呼ぶ
    public void InitVRCamObject()
    {
        var oPlayer = PlayerManager.LocalPlayer;
        var plMove = oPlayer.GetComponent<PlayerMove>();

		var cc = VRCamObject.GetComponent<CameraVRController>();
        cc.Init();

        var vca = VRCamObject.GetComponent<CameraVRAdjuster>();
        vca.Init();
    }

    public GameObject GetBaseCameraObject()
    {
		if ( VRCamObject == null )
			return null;

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
