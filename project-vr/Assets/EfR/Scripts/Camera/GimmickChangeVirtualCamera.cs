using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GimmickChangeVirtualCamera : GimmickBase
{
    [SerializeField]
    CinemachineVirtualCamera vCam;

    [SerializeField]
    CameraType cameraType=CameraType.CameraVR;

    CameraControllerBase camCon;
    // Use this for initialization
    void Start()
    {
		isCallOnlyServer = false;
        m_acTriggerEnterAction += ChangeVirtualCamera;
        switch (cameraType)
        {
            case CameraType.Camera2D:
                this.GetGameObjectWithCoroutine(CameraUtility.Camera2DName, (GameObject go) => camCon = go.GetComponent<CameraControllerBase>());
                break;
            case CameraType.CameraVR:
                this.GetGameObjectWithCoroutine(CameraUtility.CameraVRName, (GameObject go) => camCon = go.GetComponent<CameraControllerBase>());
                break;
        }
    }
    void ChangeVirtualCamera(Collider collider)
    {
        if (collider.gameObject != PlayerManager.LocalPlayer || !camCon) return;
        camCon.ChangeVirtualCamera(vCam);
    }

}
