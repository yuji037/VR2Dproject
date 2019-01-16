using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Networking;
public class GimmickChangeVirtualCameraParam : GimmickBase
{

    [SerializeField]
    Camera2DController.CameraParam cameraParam;

    Camera2DController camera2D;
    private void Start()
    {
        camera2D = GameObject.Find("Camera2D").GetComponent<Camera2DController>();
        isCallOnlyServer = false;
        m_acTriggerEnterAction += ChangeParam;
    }
    void ChangeParam(Collider collider)
    {
        if (collider.gameObject != PlayerManager.LocalPlayer) return;
        camera2D.ChangeCameraParam(cameraParam);
    }

}
