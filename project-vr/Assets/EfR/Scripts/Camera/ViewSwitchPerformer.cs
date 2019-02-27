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

    GameObject transedPosition;

    GameObject _2dCamPosition;

    GameObject VRChara;

    [SerializeField]
    AnimationCurve flipFlopMoveCurve;

    bool isInitialized = false;
    void Initialize()
    {
        C2DAdjuster.SetVRCamera(CVRAdjuster.CenterEye);
        playerNum = PlayerManager.GetPlayerNumber();
        Debug.Log(realRoom.transform.Find("TV").name);
        _2dCamPosition = realRoom.transform.Find("CamPos_2D_" + (playerNum + 1)).gameObject;
        transedPosition = _2dCamPosition.transform.Find("TransedPos").gameObject;
        VRChara = realRoom.transform.Find("VRChatCharaPos" + (playerNum + 1)).Find("VRChatChara").gameObject;
        charaSubTo2DCamPos = _2dCamPosition.transform.position - VRChara.transform.position;

        isInitialized = true;
    }
    //遷移中
    public bool IsTranslation { get; private set; }

    Vector3 charaSubTo2DCamPos;

    int playerNum;
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

    IEnumerator Translation(PlayerMove.MoveType toMoveType, System.Action callBack)
    {
        IsTranslation = true;
        CVRAdjuster.GetComponent<CameraVRController>().HasCameraAuthority = false;
        CVRAdjuster.GetComponent<CameraVRController>().enabled = false;
        C2DAdjuster.GetComponent<Camera2DController>().HasCameraAuthority = false;

        var seChannel = SoundManager.GetInstance().Play("flipflop");
        int effChannel = -1;
        var currentVRVCam = CVRAdjuster.GetComponent<CameraVRController>().CurrentVCam.transform;


        switch (toMoveType)
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
            case PlayerMove.MoveType.FIXED:
                Debug.Log("遷移開始2D→VR");

                //C2DAdjuster.GetComponent<Camera>().fieldOfView = CVRAdjuster.GetCenterEyeFOV();

                var cam2DCon = C2DAdjuster.GetComponent<Camera2DController>();
                cam2DCon.NoiseActivate(0.5f, 1.0f);
                yield return new WaitForSeconds(1.0f);

                effChannel = EffectManager.GetInstance().Play("CharaParticleAttract3", Vector3.zero, true, "VRChatCharaPos" + (playerNum + 1), "TV");


                //黒幕
                FadeInOutController._2DFadePanel.StartBlackFadeOut(0.1f);

                //2Dカメラを固定カメラ位置に
                TransformUtility.TransSamePosRot(C2DAdjuster.transform, currentVRVCam, 0.3f);
                yield return new WaitForSeconds(0.3f);

                //黒幕
                FadeInOutController._2DFadePanel.StartBlackFadeIn(0.1f);

                //リアルルーム全体を固定カメラの位置に置き、回転
                realRoom.transform.rotation = currentVRVCam.transform.rotation;
                CVRAdjuster.transform.rotation = currentVRVCam.transform.rotation;
                CVRAdjuster.transform.position = currentVRVCam.transform.position;
                var sub = CVRAdjuster.transform.position - _2dCamPosition.transform.position;
                realRoom.Translate(sub, Space.World);


                //リビングルームを移動
                animator.CrossFade("ToVR", 0f);

                yield return MoveRealRoomAndVRCharaRoutine(_2dCamPosition.transform.position - transedPosition.transform.position, 3.0f);

                EffectManager.GetInstance().Stop(effChannel, 2f);

                CVRAdjuster.ChangeVRCamParamTo2DCam();

                //2Dカメラを所定の位置に戻す
                C2DAdjuster.Set2DPosition();

                playerMove.RendererSwitchForPlayerMoveType(toMoveType);

                CVRAdjuster.GetComponent<CameraVRController>().HasCameraAuthority = true;

                Debug.Log("遷移停止2D→VR");
                break;
            case PlayerMove.MoveType._2D:
                Debug.Log("遷移開始VR→2D");

                var cVRCon = CVRAdjuster.GetComponent<CameraVRController>();
                cVRCon.NoiseActivate(0.05f, 1.0f);
                yield return new WaitForSeconds(1.0f);

                effChannel = EffectManager.GetInstance().Play("CharaParticleAttract3", Vector3.zero, true, "TV", "VRChatCharaPos" + ( playerNum + 1 ));

                playerMove.RendererSwitchForPlayerMoveType(toMoveType);

                //realRoom.transform.rotation = CVRAdjuster.transform.rotation;

                realRoom.transform.rotation = currentVRVCam.transform.rotation;
                CVRAdjuster.transform.rotation = currentVRVCam.transform.rotation;
                CVRAdjuster.transform.position = currentVRVCam.transform.position;
                var subPos = CVRAdjuster.transform.position - transedPosition.transform.position;
                realRoom.Translate(subPos, Space.World);

                CVRAdjuster.ChangeVRCamParamToDefault();
                C2DAdjuster.transform.position = CVRAdjuster.transform.position;

                animator.CrossFade("To2D", 0f);
                yield return MoveRealRoomAndVRCharaRoutine(transedPosition.transform.position - _2dCamPosition.transform.position, 3.0f, true);

                EffectManager.GetInstance().Stop(effChannel, 2f);
                FadeInOutController._2DFadePanel.StartBlackFadeOut(0.1f);

                //2DCamera位置を元の位置に
                TransformUtility.TransSamePosRot(C2DAdjuster.transform, C2DAdjuster.GetComponent<Camera2DController>().CurrentVCam.transform,0.3f);
                yield return new WaitForSeconds(0.3f);

                FadeInOutController._2DFadePanel.StartBlackFadeIn(0.1f);

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
    IEnumerator MoveRealRoomAndVRCharaRoutine(Vector3 moveVec, float duration, bool isReverseFlipFlopCurve = false)
    {
        var startPos = realRoom.transform.position;
        var endPos = startPos + moveVec;
        var prePos = startPos;


        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float curveValue = 0f;
            if (isReverseFlipFlopCurve)
            {
                curveValue = curveValue = 1.0f-flipFlopMoveCurve.Evaluate(1.0f-(t / duration));
            }
            else
            {
                curveValue = flipFlopMoveCurve.Evaluate(t / duration);
            }
            realRoom.transform.position = startPos + curveValue * moveVec;
            VRChara.transform.position = CVRAdjuster.transform.position-charaSubTo2DCamPos;
            yield return null;
        }
        realRoom.transform.position = endPos;
        VRChara.transform.position = CVRAdjuster.transform.position - charaSubTo2DCamPos;
    }
}
