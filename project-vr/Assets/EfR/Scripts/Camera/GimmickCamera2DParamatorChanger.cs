using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Networking;
public class GimmickCamera2DParamatorChanger : GimmickBase {
   
    [SerializeField]
    Camera2DController.CameraParam cameraParam;

    Camera2DController camera2D;
    private void Start()
    {
        camera2D=GameObject.Find("Camera2D").GetComponent<Camera2DController>();
        m_acTriggerEnterAction += ChangeParam;
    }
    void ChangeParam(Collider collider)
    {
        var pStatus = collider.GetComponent<PlayerStatus>();
        if (!pStatus) return;
        CmdChangeParam((int)pStatus.Number);
    }
    [Command]
    void CmdChangeParam(int playerNumber)
    {
        RpcChangeParam(playerNumber);
    }
    [ClientRpc]
    void RpcChangeParam(int playerNumber)
    {
        if (PlayerManager.LocalPlayer.GetComponent<PlayerStatus>().Number == (PlayerNumber)playerNumber)
        {
            camera2D.ChangeCameraParam(cameraParam);
        }
    }
}
