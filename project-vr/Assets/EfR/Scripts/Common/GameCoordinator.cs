using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCoordinator : MonoBehaviour {

    bool selectedVRDevice = false;
    VRObjectManager vrObjectManager;
    [SerializeField]
    EFRNetworkManager networkManager;

	// Use this for initialization
	void Start () {
        StartCoroutine(GameStartCoroutine());
        vrObjectManager = VRObjectManager.GetInstance();
    }

    void OnGUI()
    {
        if ( !selectedVRDevice )
        {
            GUI.TextField(new Rect(10, 10, 150, 30), "Select VR Device");

            if ( GUI.Button(new Rect(10, 50, 150, 30), "No Device") ) {
                SelectVRDevice(VRDeviceType.NO_DEVICE);
            }
            if ( GUI.Button(new Rect(10, 90, 150, 30), "Oculus")){
                SelectVRDevice(VRDeviceType.OCULUS);
            }
            if ( GUI.Button(new Rect(10, 130, 150, 30), "HTC Vive")){
                SelectVRDevice(VRDeviceType.HTC_VIVE);
            }
        }
    }

    void SelectVRDevice(VRDeviceType deviceType)
    {
        vrObjectManager.SetDeviceType(deviceType);
        selectedVRDevice = true;
    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitUntil(() => selectedVRDevice);

        yield return StartCoroutine(SceneLoader.IELoadScene("Root_Frame3D"));
        yield return StartCoroutine(SceneLoader.IELoadScene("Root_Stage"));
        yield return StartCoroutine(StageSceneLoader.GetInstance().LoadNextStage());

        StartCoroutine(SceneLoader.IELoadScene("Root_UI"));

        vrObjectManager.SpawnVRCamObject();
        networkManager.gameObject.SetActive(true);

        yield return new WaitUntil(() => networkManager.IsClientSceneReady());
        networkManager.SpawnPlayer();
        yield return new WaitUntil(() => PlayerManager.LocalPlayer != null);

        OnPlayerSpawned();
    }

    public void OnPlayerSpawned()
    {
        //PlayerManager.LocalPlayerInit();
        vrObjectManager.InitVRCamObject();

        Debug.Log("Init VRCam and Player");
    }
}
