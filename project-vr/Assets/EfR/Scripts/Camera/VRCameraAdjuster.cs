using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VRCameraAdjuster : MonoBehaviour
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

    Camera skyCam;
    Camera SkyCam
    {
        get
        {
            if (!skyCam)
            {
                skyCam = GameObject.Find("SkyboxCamera").GetComponent<Camera>();
            }
            return skyCam;
        }
    }

    [SerializeField]
    CameraParam realWorldCameraParam;

    [SerializeField]
    CameraParam videoGameWorldCameraParam;

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
                SetAllVRCamsParam(videoGameWorldCameraParam);
                break;
            case PlayerMove.MoveType._2D:
                SetAllVRCamsParam(realWorldCameraParam);
                break;
        }
        Debug.Log("Camera Init");
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
        SkyCam.transform.eulerAngles = Vector3.zero;
    }
    //VRCameraを2DCameraと同じにする状態にする。
    public void ChangeVRCamParamTo2DCam(Vector3 Camera2DPos)
    {
        //ゲームカメラにリアルカメラのポジションを合わせる
        transform.position = Camera2DPos;
        //transform.eulerAngles = GameCam.transform.eulerAngles /*+ realCam.transform.eulerAngles*/;

        //gameCamと同じものを見る設定
        SetAllVRCamsParam(videoGameWorldCameraParam);

        //skyCamのターゲットを外す
        SkyCam.targetTexture = null;

        //GetComponent<ControlCameraOVRRig>().enabled = true;
    }


    //VRCamをデフォルトの値に変更
    public void ChangeVRCamParamToDefault()
    {


        //skyCamのターゲット(TV画面)を入れなおす
        SkyCam.targetTexture = targetTexture;

        //realCamのdefaultの設定に
        SetAllVRCamsParam(realWorldCameraParam);
        transform.position = NearTVObject.position;

        //コントロールを無効に
        //GetComponent<ControlCamera>().enabled = false;

    }


    //RealCamをTVから離す
    public void DepartTV(float duration)
    {
        StartCoroutine(DepartTVCoroutine(duration));
    }

    IEnumerator DepartTVCoroutine(float duration)
    {

        //gameCamを平行投影にすると180度回転するので再度回転させる
        SkyCam.transform.eulerAngles = new Vector3(0, 0, 180);
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
    public void TransPosition(Vector3 endPos)
    {
        StartCoroutine(AdjustPosition(transform, endPos));
    }

    IEnumerator AdjustPosition(Transform adjustTarget, Vector3 endPos)
    {
        Vector3 defPos = adjustTarget.position;
        //Vector3 defRot = adjustTarget.eulerAngles;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            adjustTarget.position = defPos * (1f - t) / 1f + endPos * t / 1f;
            //adjustTarget.eulerAngles = defRot * (1f - t) / 1f + endRot * t / 1f;
            yield return null;
        }
    }
}
