using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraAttachTarget : MonoBehaviour
{
    CinemachineVirtualCamera vCam;
    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }
    bool isAttached;
    // Update is called once per frame
    void Update()
    {
        if (!isAttached&&PlayerManager.LocalPlayer&&PlayerManager.playerMove.isReady)
        {
            if (AttachTarget(PlayerManager.LocalPlayer))
            {
                
            }
            else if(AttachTarget(PlayerManager.OtherPlayer))
            {
                StageSwitchRenderer.GetInstance().SwitchRenderer(1,true);
                StageSwitchRenderer.GetInstance().SwitchRenderer(2, false);
            }
        }
    }
    bool AttachTarget(GameObject target)
    {
        if (target&&
            target.GetComponent<PlayerStatus>().Active)
        {
            vCam.Follow = target.transform;
            isAttached = true;
            return true;
        }
        return false;
    }
}
