using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ToDo:2D→TPSorFPSに出来るように設計
public class ViewSwitchPerformer : SingletonMonoBehaviour<ViewSwitchPerformer>
{

    // Use this for initialization
    [SerializeField]
    VRCameraAdjuster RCAdjuster;
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
        if(!_2dCamera)_2dCamera= C2DAdjuster.GetComponent<Camera>();
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
            if (!pm)pm= PlayerManager.LocalPlayer.GetComponent<PlayerMove>();
            return pm;
        }
    }

    bool isInitialized = false;
    void Initialize()
    {
        C2DAdjuster.SetDefaultFov(RCAdjuster.GetCenterEyeFOV());
        isInitialized = true;
    }


    //遷移中
    public bool IsTranslation { get; private set; }


    public void SwitchView(PlayerMove.MoveType switchType,System.Action callBack=null)
    {
        if (IsTranslation) return;
        if (!isInitialized) Initialize();
        StartCoroutine(Translation(switchType, callBack));
    }

    IEnumerator Translation(PlayerMove.MoveType moveType,System.Action callBack)
    {
        IsTranslation = true;
        switch (moveType)
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
                Debug.Log("遷移開始Real→Virtual");

                RCAdjuster.PlayMotionBlur();

                transRealParticle.Play();
                //TVにVRCameraを近づける
                RCAdjuster.ApproachTV(1.0f);
                yield return new WaitForSeconds(1.0f);

                //realCamがTV画面まで近づいたら遠近感を出す。
                C2DAdjuster.Trans3DPerspective();
                yield return new WaitForSeconds(1.0f);


                //2dCameraを徐々にプレイヤー位置に近づける
                C2DAdjuster.TransPosition(viewPointStorage.GetCamPos(moveType),1.0f);
                yield return new WaitForSeconds(1.0f);

                //2dCameraがプレイヤー位置まで近づいたらVRCameraを同じ位置に
                RCAdjuster.ChangeVRCamParamTo2DCam(viewPointStorage.GetCamPos(moveType).position);
                transRealParticle.Stop();

                //2Dカメラを所定の位置に戻す
                C2DAdjuster.Set2DPosition();

                RCAdjuster.StopMotionBlur();

                playerMove.RendererSwitchForPlayerMoveType(moveType);

                Debug.Log("遷移停止Real→Virtual");
                break;
            case PlayerMove.MoveType._2D:
                Debug.Log("遷移開始Virtual→Real");

                RCAdjuster.PlayMotionBlur();

                playerMove.RendererSwitchForPlayerMoveType(moveType);
                
                yield return new WaitForSeconds(1.0f);

                //2DCamera位置にVRCamera移動
                RCAdjuster.TransPosAndRotToEqualize(RCAdjuster.transform,C2DAdjuster.transform);
                yield return new WaitForSeconds(1.0f);

                transRealParticle.Play();
                //VRCameraをRealWorldに戻す
                RCAdjuster.ChangeVRCamParamToDefault();

                //カメラを平行投影へ
                C2DAdjuster.Trans2DPerspective();
                yield return new WaitForSeconds(1.0f);

                //ＴＶ画面から離れる
                RCAdjuster.DepartTV(1.0f);
                yield return new WaitForSeconds(1.0f);
                RCAdjuster.StopMotionBlur();
                transRealParticle.Stop();

                Debug.Log("遷移停止Virtual→Real");
                break;
        }
        IsTranslation = false;
        if (callBack!=null) callBack();
    }
}
