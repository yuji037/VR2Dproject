using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GimmickChangeVirtualCamera : GimmickBase
{
    [SerializeField]
    CinemachineVirtualCamera vCam;

    CameraVRController vrCam;

   

    // Use this for initialization
    void Start()
    {
        m_acTriggerEnterAction += ChangeVirtualCamera;
        this.GetGameObjectWithCoroutine(CameraUtility.CameraVRName, (GameObject go) => vrCam = go.GetComponent<CameraVRController>());
    }
    void ChangeVirtualCamera(Collider collider)
    {
        if (collider.gameObject != PlayerManager.LocalPlayer || !vrCam) return;
        vrCam.ChangeVirtualCamera(vCam);
    }

}
