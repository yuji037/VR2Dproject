using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//ToDo:2D→TPSorFPSに出来るように設計
public class ViewSwitchPerformer : SingletonMonoBehaviour<ViewSwitchPerformer>
{

    // Use this for initialization
    CameraVRAdjuster CVRAdjuster;
    Camera2DAdjuster gcAd;
    Camera2DAdjuster C2DAdjuster
    {
        get
        {
            if (!gcAd) gcAd = GameObject.Find("Camera2D").GetComponent<Camera2DAdjuster>();
            return gcAd;
        }
    }
    Camera _2dCamera;
    public Camera Get2DCamera()
    {
        if (!_2dCamera) _2dCamera = C2DAdjuster.GetComponent<Camera>();
        return _2dCamera;
    }

    ParticleSystem trp;
    ParticleSystem transRealParticle
    {
        get
        {
            if (!trp) trp = GameObject.Find("TransRealParticle").GetComponent<ParticleSystem>();
            return trp;
        }
    }


    ParticleSystem tgp;
    ParticleSystem transGameParticle
    {
        get
        {
            if (!tgp) tgp = GameObject.Find("TransGameParticle").GetComponent<ParticleSystem>();
            return tgp;
        }
    }

    ViewPointStorage vps;
    ViewPointStorage viewPointStorage
    {
        get
        {
            if (!vps) vps = PlayerManager.LocalPlayer.GetComponent<ViewPointStorage>();
            return vps;
        }
    }

    PlayerMove pm;
    PlayerMove playerMove
    {
        get
        {
            if (!pm) pm = PlayerManager.LocalPlayer.GetComponent<PlayerMove>();
            return pm;
        }
    }

    bool isInitialized = false;
    void Initialize()
    {
        C2DAdjuster.SetDefaultFov(CVRAdjuster.GetCenterEyeFOV());
        isInitialized = true;
    }


    //遷移中
    public bool IsTranslation { get; private set; }

    private void Start()
    {
        CVRAdjuster = GetComponent<CameraVRAdjuster>();
    }
    public void SwitchView(PlayerMove.MoveType switchType, System.Action callBack = null)
    {
        if (IsTranslation) return;
        if (!isInitialized) Initialize();
        StartCoroutine(Translation(switchType, callBack));
    }

    IEnumerator Translation(PlayerMove.MoveType moveType, System.Action callBack)
    {
        IsTranslation = true;
        CVRAdjuster.GetComponent<CameraVRController>().HasCameraAuthority = false;
        CVRAdjuster.GetComponent<CameraVRController>().enabled = false;
        C2DAdjuster.GetComponent<Camera2DController>().HasCameraAuthority = false;
       
        switch (moveType)
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
            case PlayerMove.MoveType.FIXED:
                Debug.Log("遷移開始2D→VR");

                transRealParticle.Play();
                //TVにVRCameraを近づける
                CVRAdjuster.ApproachTV(1.0f);
                yield return new WaitForSeconds(1.0f);

                //realCamがTV画面まで近づいたら遠近感を出す。
                //C2DAdjuster.SetDefaultFov(RCAdjuster.GetCenterEyeFOV());
                C2DAdjuster.Trans3DPerspective();
                yield return new WaitForSeconds(1.0f);


                //2dCameraを徐々にプレイヤー位置に近づける
               TransformUtility.TransPosAndRotToEqualize(C2DAdjuster.transform,viewPointStorage.GetCamPos(moveType));
                yield return new WaitForSeconds(1.0f);

                //2dCameraがプレイヤー位置まで近づいたらVRCameraを同じ位置に
                CVRAdjuster.ChangeVRCamParamTo2DCam(viewPointStorage.GetCamPos(moveType));
                transRealParticle.Stop();

                //2Dカメラを所定の位置に戻す
                C2DAdjuster.Set2DPosition();

                playerMove.RendererSwitchForPlayerMoveType(moveType);

                CVRAdjuster.GetComponent<CameraVRController>().HasCameraAuthority = true;

                Debug.Log("遷移停止2D→VR");
                break;
            case PlayerMove.MoveType._2D:
                Debug.Log("遷移開始VR→2D");

                playerMove.RendererSwitchForPlayerMoveType(moveType);

                yield return new WaitForSeconds(1.0f);

                //2DCamera位置にVRCamera移動
                Debug.Log(CVRAdjuster + ":" + C2DAdjuster);
                TransformUtility.TransPosAndRotToEqualize(CVRAdjuster.transform, C2DAdjuster.transform);
                yield return new WaitForSeconds(1.0f);

                transRealParticle.Play();
                //VRCameraをRealWorldに戻す
                CVRAdjuster.ChangeVRCamParamToDefault();

                //カメラを平行投影へ
                C2DAdjuster.GetComponent<Camera>().orthographic = true;
                C2DAdjuster.SetDefaultFov(CVRAdjuster.GetCenterEyeFOV());
                C2DAdjuster.Trans2DPerspective();


                //ＴＶ画面から離れる
                CVRAdjuster.DepartTV(1.0f);
                yield return new WaitForSeconds(1.0f);
                transRealParticle.Stop();      

                Debug.Log("遷移停止VR→2D");
                break;
        }
        CVRAdjuster.GetComponent<CameraVRController>().enabled = true;
        C2DAdjuster.GetComponent<Camera2DController>().HasCameraAuthority = true;
        IsTranslation = false;
        if (callBack != null) callBack();
    }
}
