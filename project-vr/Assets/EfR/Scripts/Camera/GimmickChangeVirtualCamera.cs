using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GimmickChangeVirtualCamera : GimmickBase
{
    [SerializeField]
    CinemachineVirtualCamera vCam;

    CameraVRController vrCam;

    [SerializeField]
    Color CollisionBoxColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = CollisionBoxColor;
        Gizmos.DrawCube(transform.position,transform.lossyScale);
    }

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
