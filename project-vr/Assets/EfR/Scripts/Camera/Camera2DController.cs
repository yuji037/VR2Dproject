using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera2DController : CameraControllerBase
{
    [System.Serializable]
    public struct CameraParam
    {
        public float offSetWidth;
        public float offSetHeight;
        public float screenX;
        public float screenY;
        public float angle;
    }

    public enum WorldDirection
    {
        Front,
        Back,
        Left,
        Right
    }

    public void ChangeCameraParam(CameraParam param)
    {
        var cft = CurrentVCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        cft.m_DeadZoneWidth = param.offSetWidth;
        cft.m_DeadZoneHeight = param.offSetHeight;
        cft.m_ScreenX = param.screenX;
        cft.m_ScreenY = param.screenY;
        ChangeCameraDirection(param.angle);
    }

    public void ChangeTargetToLocalPlayer()
    {
        if (CurrentVCam) CurrentVCam.Follow = PlayerManager.LocalPlayer.transform;
    }

    public void ChangeCameraDirection(WorldDirection cameraDirection)
    {
        ChangeCameraDirection(ConvertToAngle(cameraDirection));
    }

    public void ChangeCameraDirection(float cameraAngle)
    {
        CurrentVCam.transform.eulerAngles = new Vector3(0, cameraAngle, 0);
    }

    float ConvertToAngle(WorldDirection direction)
    {
        switch (direction)
        {
            case WorldDirection.Front:
                return 0f;
            case WorldDirection.Back:
                return 180.0f;
            case WorldDirection.Left:
                return 90.0f;
            case WorldDirection.Right:
                return -90.0f;
        }
        return 0f;
    }
}
