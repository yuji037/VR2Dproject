using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVRAdjuster : MonoBehaviour
{
    [System.Serializable]
    class CameraParam
    {
        public CameraClearFlags ClearFlags;
        public LayerMask CullingMask;
    }
    Vector3 farTVPos;

    Transform nearTVObject;
    Transform NearTVObject
    {
        get
        {
            if (!nearTVObject) nearTVObject = GameObject.Find("NearTVObject").transform;
            return nearTVObject;
        }
    }

    [SerializeField]
    RenderTexture targetTexture;

    Camera[] VRCams;


    [SerializeField]
    CameraParam realWorldCameraParam;

    [SerializeField]
    CameraParam videoGameWorldCameraParam;

    [SerializeField]
    Camera centerEye;

    public Camera CenterEye
    {
        get { return centerEye; }
    }


    UnityEngine.PostProcessing.PostProcessingBehaviour postProcessing;

    public void Init()
    {
        var player = PlayerManager.LocalPlayer;
        VRCams = GetComponentsInChildren<Camera>();
        postProcessing = GetComponentInChildren<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        farTVPos = player.GetComponent<ViewPointStorage>().GetCamPos(PlayerMove.MoveType._2D).position;
        var moveType = player.GetComponent<PlayerMove>().moveType;
        switch (moveType)
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
            case PlayerMove.MoveType.FIXED:
                SetAllVRCamsParam(videoGameWorldCameraParam);
                break;
            case PlayerMove.MoveType._2D:
                SetAllVRCamsParam(realWorldCameraParam);
                break;
        }
        Debug.Log("Camera Init");
    }

    public float GetCenterEyeFOV()
    {
        return CenterEye.fieldOfView;
    }
    
        void SetAllVRCamsParam(CameraParam cameraParam)
    {
        foreach (var VRCam in VRCams)
            SetCameraParam(VRCam, cameraParam);
    }

    void SetCameraParam(Camera camera, CameraParam cameraParam)
    {
        camera.cullingMask = cameraParam.CullingMask;
        camera.clearFlags = cameraParam.ClearFlags;
    }
    public void PlayMotionBlur()
    {
        postProcessing.enabled = true;
    }
    public void StopMotionBlur()
    {
        postProcessing.enabled = false;
    }
    //TVに近づく
    public void ApproachTV(float duration)
    {
        StartCoroutine(ApproachTVRoutine(duration));
    }
    IEnumerator ApproachTVRoutine(float duration)
    {
        yield return MoveThis(farTVPos, NearTVObject.position, duration);
        //SkyCam.transform.eulerAngles = Vector3.zero;
    }
    public void ChangeVRCamPosRotTo(Transform target)
    {
        transform.position = target.position;
        transform.eulerAngles = target.transform.eulerAngles;
    }

    //VRCameraを2DCameraと同じにする状態にする。
    public void ChangeVRCamParamTo2DCam()
    { 
        SetAllVRCamsParam(videoGameWorldCameraParam);
    }


    //VRCamをデフォルトの値に変更
    public void ChangeVRCamParamToDefault()
    {
        //realCamのdefaultの設定に
        SetAllVRCamsParam(realWorldCameraParam);
        transform.position = NearTVObject.position;
        transform.rotation = NearTVObject.rotation;
    }


    //RealCamをTVから離す
    public void DepartTV(float duration)
    {
        StartCoroutine(DepartTVCoroutine(duration));
    }

    IEnumerator DepartTVCoroutine(float duration)
    {
        yield return StartCoroutine(MoveThis(NearTVObject.position, farTVPos, duration));
    }

    IEnumerator MoveThis(Vector3 start, Vector3 end,float duration)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.position = start * (duration - t) / duration + end * t / duration;
            yield return null;
        }
    }
}
