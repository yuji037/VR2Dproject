using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera2DController : CameraControllerBase
{
    [SerializeField]
    float offsetDumping=0.05f;

    [System.Serializable]
    public struct CameraParam
    {
        public float offSetWidth;
        public float offSetHeight;
        public float screenX;
        public float screenY;
        public float angle;
        [SerializeField, Header("0以下だと、cameraの既存の値が使われる")]
        public float distance;
    }

    float nextScreenX=-1.0f;
    float nextScreenY=-1.0f;


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
        if (!cft) return;
        cft.m_DeadZoneWidth = param.offSetWidth;
        cft.m_DeadZoneHeight = param.offSetHeight;
        nextScreenX = param.screenX;
        nextScreenY = param.screenY;
        if (param.distance > 0) cft.m_CameraDistance = param.distance;
        ChangeCameraDirection(param.angle);
    }
    private void Update()
    {
        var cft = CurrentVCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (cft &&
            nextScreenX > -1.0f&&
            nextScreenY > -1.0f)
        {
            cft.m_ScreenX = Mathf.Lerp(cft.m_ScreenX, nextScreenX, offsetDumping);
            cft.m_ScreenY = Mathf.Lerp(cft.m_ScreenY, nextScreenY, offsetDumping);
        }
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
