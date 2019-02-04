using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDefaultVirtualCamera : MonoBehaviour {

    [SerializeField]
    CameraType cameraType;

    private void Start()
    {
        string cameraObjName="";
        switch (cameraType)
        {
            case CameraType.Camera2D:
                cameraObjName = CameraUtility.Camera2DName;
                break;
            case CameraType.CameraVR:
                cameraObjName = CameraUtility.CameraVRName;
                break;
        }
        this.GetGameObjectWithCoroutine(cameraObjName,(x)=>x.GetComponent<CameraControllerBase>().ChangeVirtualCamera(GetComponent<Cinemachine.CinemachineVirtualCamera>()));
    }
}
