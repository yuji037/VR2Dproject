using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera2DController : MonoBehaviour
{
    [System.Serializable]
    public struct CameraParam
    {
      public float offSetWidth;
      public float offSetHeight;
      public float screenX;
      public float screenY;
    }

    public enum WorldDirection
    {
        Front,
        Back,
        Left,
        Right
    }
    GameObject _targetObj;
    GameObject targetObject
    {
        get
        {
            if (!_targetObj)
                _targetObj = PlayerManager.LocalPlayer;
            return _targetObj;
        }
    }

    [SerializeField]
    CinemachineVirtualCamera[] vCamArray;

    CinemachineVirtualCamera currentVCam;


    bool isInitialized =false;

    void SetAngle()
    {
        if (PlayerManager.CheckLocalPlayerNumber(PlayerNumber.Player1))
        {
            ChangeCameraDirection(WorldDirection.Back);
        }
        else
        {
            ChangeCameraDirection(WorldDirection.Front);
        }
    }
    private void Init()
    {
        if (isInitialized||!PlayerManager.LocalPlayer.GetComponent<PlayerStatus>().Initialized) return;
        Debug.Log("Init "+this);
        ChangeVirtualCamera(0);
        isInitialized = true;
    }

    void ChangeVirtualCamera(int number)
    {
        if (number < 0 || vCamArray.Length <= number) return;
        if (currentVCam) currentVCam.Priority = 0;
        currentVCam = vCamArray[number];
        currentVCam.Priority = 1;
        SetAngle();
    }
    public void ChangeCameraParam(CameraParam param)
    {
        var cft=currentVCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        cft.m_DeadZoneWidth = param.offSetWidth;
        cft.m_DeadZoneHeight = param.offSetHeight;
        cft.m_ScreenX = param.screenX;
        cft.m_ScreenY = param.screenY;
    }

    void Update()
    {
        if (!targetObject) return;
        Init();
        if (!isInitialized) return;
    }

    public bool HasCameraAuthority
    {
        get
        {
            return GetComponent<CinemachineBrain>().enabled;
        }
        set
        {
            GetComponent<CinemachineBrain>().enabled= value;
        }
    }

    public void ChangeCameraDirection(WorldDirection cameraDirection)
    {
        ChangeCameraDirection(ConvertToAngle(cameraDirection));
    }

    public void ChangeCameraDirection(float cameraAngle)
    {
        currentVCam.transform.eulerAngles = new Vector3(0,cameraAngle,0);
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
