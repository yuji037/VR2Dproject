using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraSetUp : MonoBehaviour {
    CinemachineVirtualCamera vCam;
    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }
    bool isAttached;	
	// Update is called once per frame
	void Update () {
        if (!isAttached && PlayerManager.LocalPlayer)
        {
            vCam.Follow = PlayerManager.LocalPlayer.transform;
            isAttached = true;
        }
	}
}
