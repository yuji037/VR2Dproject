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

    [SerializeField]
    Transform nearTVPos;

    [SerializeField]
    RenderTexture targetTexture;

    Camera VRCam;

    Camera videoGameCam;
    Camera VideoGameCam
    {
        get
        {
            if (!videoGameCam)
            {
                videoGameCam = GameObject.Find("GameCamera").GetComponent<Camera>();
            }
            return videoGameCam;
        }
    }

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
    private void Start()
    {
        DelayInit(1.0f);
    }
    IEnumerator DelayInit(float delay)
    {
        yield return new WaitForSeconds(delay);
        Init();
    }

    void Init()
    {
        var player=PlayerManager.LocalPlayer;
        VRCam = GetComponent<Camera>();
        postProcessing = VRCam.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        farTVPos = player.GetComponent<ViewPointStorage>().GetCamPos(PlayerMove.MoveType._2D).position;
        var moveType = player.GetComponent<PlayerMove>().moveType;
        switch (moveType)
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
                SetCameraParam(VRCam, videoGameWorldCameraParam);
                break;
            case PlayerMove.MoveType._2D:
                SetCameraParam(VRCam, realWorldCameraParam);
                break;
        }
    }

    void SetCameraParam(Camera camera,CameraParam cameraParam)
    {
        camera.cullingMask = cameraParam.CullingMask;
        camera.clearFlags = cameraParam.ClearFlags;
    }

    //TVに近づく
    public void ApproachTV()
    {
        postProcessing.enabled = true;
        StartCoroutine(ApproachTVRoutine());
    }
    IEnumerator ApproachTVRoutine()
    {
        yield return MoveThis(farTVPos, nearTVPos.position);
        SkyCam.transform.eulerAngles = Vector3.zero;
    }
    //VRCameraを2DCameraと同じにする状態にする。
    public void ChangeVRCamParamTo2DCam()
    {
        //ゲームカメラにリアルカメラのポジションを合わせる
        transform.position = VideoGameCam.transform.position;
        //transform.eulerAngles = GameCam.transform.eulerAngles /*+ realCam.transform.eulerAngles*/;

        //gameCamと同じものを見る設定
        SetCameraParam(VRCam,videoGameWorldCameraParam);

        //skyCamのターゲットを外す
        SkyCam.targetTexture = null;

        //GetComponent<ControlCameraOVRRig>().enabled = true;
        postProcessing.enabled = false;
    }


    //VRCamをデフォルトの値に変更
    public void ChangeVRCamParamToDefault()
    {
        postProcessing.enabled = true;

        //skyCamのターゲット(TV画面)を入れなおす
        SkyCam.targetTexture = targetTexture;

        //realCamのdefaultの設定に
        SetCameraParam(VRCam, realWorldCameraParam);
        transform.position = nearTVPos.position;

        //コントロールを無効に
        //GetComponent<ControlCamera>().enabled = false;

    }
    

    //RealCamをTVから話す
    public void DepartTV()
    {
        StartCoroutine(DepartTVCoroutine());
    }

    IEnumerator DepartTVCoroutine()
    {

        //gameCamを平行投影にすると180度回転するので再度回転させる
        SkyCam.transform.eulerAngles = new Vector3(0, 0, 180);
        yield return StartCoroutine(MoveThis(nearTVPos.position, farTVPos));

    }

    IEnumerator MoveThis(Vector3 start, Vector3 end)
    {
        float dur = 2f;
        for (float t = 0; t < dur; t += Time.deltaTime)
        {
            transform.position = start * (dur - t) / dur + end * t / dur;
            yield return null;
        }
    }
    public void TransPosition(Vector3 endPos,Vector3 endRot)
    {
        StartCoroutine(AdjustPosition(transform, endPos, endRot));
    }

    IEnumerator AdjustPosition(Transform adjustTarget, Vector3 endPos, Vector3 endRot)
    {
        Vector3 defPos = adjustTarget.position;
        Vector3 defRot = adjustTarget.eulerAngles;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            adjustTarget.position = defPos * (1f - t) / 1f + endPos * t / 1f;
            adjustTarget.eulerAngles = defRot * (1f - t) / 1f + endRot * t / 1f;
            yield return null;
        }
    }
}
