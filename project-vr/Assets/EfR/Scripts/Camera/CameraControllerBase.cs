using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControllerBase : MonoBehaviour {

    public CinemachineVirtualCamera CurrentVCam { get; private set; }

    public bool HasCameraAuthority
    {
        get
        {
            return GetComponent<CinemachineBrain>().enabled;
        }
        set
        {
            GetComponent<CinemachineBrain>().enabled = value;
        }
    }


    public void ChangeVirtualCamera(CinemachineVirtualCamera vCam)
    {
        Debug.Log("Change VirtualCamera" + vCam);
        if (CurrentVCam == vCam) return;
        if (CurrentVCam) CurrentVCam.enabled = false;
        CurrentVCam = vCam;
        CurrentVCam.enabled = true;
        OnChangedVirtualCamera();
    }

    protected virtual void OnChangedVirtualCamera()
    {
    }
}
