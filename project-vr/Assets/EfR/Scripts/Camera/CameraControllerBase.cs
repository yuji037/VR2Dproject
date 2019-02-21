using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControllerBase : MonoBehaviour
{
    noisefx _noisefx;

    noisefx NoiseFx
    {
        get {
            if (!_noisefx) _noisefx = GetComponentInChildren<noisefx>();
            return _noisefx;
        }
    }

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
    public void NoiseActivate(float intensity,float duration)
    {
        StartCoroutine(NoiseRoutine(intensity,duration));
    }
    IEnumerator NoiseRoutine(float intensity,float duration)
    {
        NoiseFx.enabled = true;
        NoiseFx.intensity = intensity;
        yield return new WaitForSeconds(duration);
        NoiseFx.enabled = false;
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
