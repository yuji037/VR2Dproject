using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    [SerializeField]
    Transform realRoom;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Transform transedPos;

    [SerializeField]
    GameObject[] _2dCamPositions;

    bool isInitialized = false;
    void Initialize()
    {
        C2DAdjuster.SetVRCamera(CVRAdjuster.CenterEye);
        isInitialized = true;
    }
    //遷移中
    public bool IsTranslation { get; private set; }

    private void Start()
    {
        this.GetGameObjectWithCoroutine(CameraUtility.CameraVRName, (obj) => CVRAdjuster = obj.GetComponent<CameraVRAdjuster>());
    }
    public void SwitchView(PlayerMove.MoveType switchType, System.Action callBack = null)
    {
        if (IsTranslation) return;
        if (!isInitialized) Initialize();
        StartCoroutine(Translation(switchType, callBack));
    }

    //trueだと遷移時VRカメラが動く
    bool isCameraVRTranslation = false;

    IEnumerator Translation(PlayerMove.MoveType toMoveType, System.Action callBack)
    {
        IsTranslation = true;
        CVRAdjuster.GetComponent<CameraVRController>().HasCameraAuthority = false;
        CVRAdjuster.GetComponent<CameraVRController>().enabled = false;
        C2DAdjuster.GetComponent<Camera2DController>().HasCameraAuthority = false;

		var seChannel = SoundManager.GetInstance().Play("flipflop");
        var currentVRVCam = CVRAdjuster.GetComponent<CameraVRController>().CurrentVCam.transform;

        switch ( toMoveType)
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
            case PlayerMove.MoveType.FIXED:
                Debug.Log("遷移開始2D→VR");

                //C2DAdjuster.GetComponent<Camera>().fieldOfView = CVRAdjuster.GetCenterEyeFOV();

                var cam2DCon= C2DAdjuster.GetComponent<Camera2DController>();
                cam2DCon.NoiseActivate(0.5f,1.0f);
                yield return new WaitForSeconds(1.0f);

                //2Dカメラを固定カメラ位置に
                TransformUtility.TransSamePosRot(C2DAdjuster.transform,currentVRVCam,0.5f);
                yield return new WaitForSeconds(0.5f);

                //黒幕
                FadeInOutController._2DFadePanel.StartBlackFadeOut(0.2f);
                yield return new WaitForSeconds(0.5f);

                //リアルルーム全体を固定カメラの位置に置き、回転
                //ステージ内の移行先カメラと今のカメラリグ位置の差分を取り、
                realRoom.transform.rotation = currentVRVCam.transform.rotation;
                CVRAdjuster.transform.rotation = currentVRVCam.transform.rotation;
                CVRAdjuster.transform.position = currentVRVCam.transform.position;
                var sub = CVRAdjuster.transform.position - _2dCamPositions[PlayerManager.GetPlayerNumber()].transform.position;
                realRoom.Translate(sub, Space.World);
                FadeInOutController._2DFadePanel.StartBlackFadeIn(0.2f);


                //リビングルームを移動
                animator.CrossFade("ToVR",0f);

                yield return new WaitForSeconds(3.0f);
                CVRAdjuster.ChangeVRCamParamTo2DCam();
                //if (isCameraVRTranslation)
                //{
                //    //VRCameraを2Dカメラの位置から、徐々にmoveTypeの視点の位置に近づける
                //    CVRAdjuster.ChangeVRCamPosRotTo(C2DAdjuster.transform);
                //    CVRAdjuster.ChangeVRCamParamTo2DCam();
                //    TransformUtility.TransSamePosRot(CVRAdjuster.transform, viewPointStorage.GetCamPos(toMoveType));
                //    yield return new WaitForSeconds(1.0f);
                //}
                //else
                //{

                //    TransformUtility.TransSamePosRot(C2DAdjuster.transform,
                //       () =>
                //       {
                //           var diff = CVRAdjuster.CenterEye.transform.position - CVRAdjuster.transform.position;
                //           return viewPointStorage.GetCamPos(toMoveType).position + diff;
                //       },
                //       () =>
                //       {
                //           return viewPointStorage.GetCamPos(toMoveType).rotation*CVRAdjuster.CenterEye.transform.localRotation;
                //       }
                //        );
                //    yield return new WaitForSeconds(1.0f);
                //    CVRAdjuster.ChangeVRCamParamTo2DCam();
                //}


                //2Dカメラを所定の位置に戻す
                C2DAdjuster.Set2DPosition();

                playerMove.RendererSwitchForPlayerMoveType(toMoveType);

                CVRAdjuster.GetComponent<CameraVRController>().HasCameraAuthority = true;

                Debug.Log("遷移停止2D→VR");
                break;
            case PlayerMove.MoveType._2D:
                Debug.Log("遷移開始VR→2D");

                var cVRCon = CVRAdjuster.GetComponent<CameraVRController>();
                cVRCon.NoiseActivate(0.05f,1.0f);
                yield return new WaitForSeconds(1.0f);

                playerMove.RendererSwitchForPlayerMoveType(toMoveType);

                //realRoom.transform.rotation = CVRAdjuster.transform.rotation;

                realRoom.transform.rotation = currentVRVCam.transform.rotation;
                CVRAdjuster.transform.rotation = currentVRVCam.transform.rotation;
                CVRAdjuster.transform.position = currentVRVCam.transform.position;
                var subPos = CVRAdjuster.transform.position - transedPos.position;
                realRoom.Translate(subPos, Space.World);

                CVRAdjuster.ChangeVRCamParamToDefault();
                C2DAdjuster.transform.position = CVRAdjuster.transform.position;

                animator.CrossFade("To2D",0f);
                yield return new WaitForSeconds(3.0f);

                FadeInOutController._2DFadePanel.StartBlackFadeIn(0.2f);
                yield return new WaitForSeconds(0.2f);

                //2DCamera位置を元の位置に
                TransformUtility.TransSamePosRot(C2DAdjuster.transform, C2DAdjuster.GetComponent<Camera2DController>().CurrentVCam.transform);
                yield return new WaitForSeconds(1.0f);

                Debug.Log("遷移停止VR→2D");
                break;
        }
        CVRAdjuster.GetComponent<CameraVRController>().enabled = true;
        C2DAdjuster.GetComponent<Camera2DController>().HasCameraAuthority = true;
        IsTranslation = false;
        if (callBack != null) callBack();

		SoundManager.GetInstance().Fadeout(seChannel);
		SoundManager.GetInstance().UpdateBGMParam();
	}
}
