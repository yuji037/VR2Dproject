using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ToDo:2D→TPSorFPSに出来るように設計
public class ViewSwitchPerformer : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    VRCameraAdjuster RCAdjuster;
    Camera2DAdjuster gcAd;
    Camera2DAdjuster C2DAdjuster
    {
        get
        {
            if (!gcAd)
            {
                gcAd = GameObject.Find("GameCamera").GetComponent<Camera2DAdjuster>();
            }
            return gcAd;
        }
    }
    [SerializeField]
    ParticleSystem transRealParticle;

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
    //遷移中
    public bool IsTranslation { get; private set; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwitchView(PlayerManager.LocalPlayer.GetComponent<PlayerMove>().moveType);
        }
    }

    void SwitchView(PlayerMove.MoveType switchType)
    {
        if (IsTranslation) return;
        StartCoroutine(Translation(switchType));
    }

    IEnumerator Translation(PlayerMove.MoveType moveType)
    {
        IsTranslation = true;
        switch (moveType)
        {
            case PlayerMove.MoveType.FPS:
                break;
            case PlayerMove.MoveType.TPS:
                Debug.Log("遷移開始Real→Virtual");
                //TVにVRCameraを近づける
                RCAdjuster.ApproachTV();
                yield return new WaitForSeconds(1.0f);

                //realCamがTV画面まで近づいたら遠近感を出す。
                C2DAdjuster.Trans3DPerspective();
                yield return new WaitForSeconds(1.0f);

                //2dCameraを徐々にプレイヤー位置に近づける
                C2DAdjuster.TransPosition(vps.GetCamPos(moveType));
                yield return new WaitForSeconds(2.1f);
                
                //2dCameraがプレイヤー位置まで近づいたらVRCameraを同じ位置に
                RCAdjuster.ChangeVRCamParamTo2DCam();
                
                //2Dカメラを所定の位置に戻す
                C2DAdjuster.Set2DPosition();
                Debug.Log("遷移停止Real→Virtual");
                break;
            case PlayerMove.MoveType._2D:
                Debug.Log("遷移開始Virtual→Real");
                
                //2ＤCamera位置にVRCamera移動
                RCAdjuster.TransPosition(C2DAdjuster.Get2DCameraPos(),C2DAdjuster.DefaultRot);
                yield return new WaitForSeconds(1.0f);

                //VRCameraをRealWorldに戻す
                RCAdjuster.ChangeVRCamParamToDefault();

                //カメラを平行投影へ
                C2DAdjuster.Trans2DPerspective();
                
                //ＴＶ画面から離れる
                RCAdjuster.DepartTV();
                Debug.Log("遷移停止Virtual→Real");
                break;
        }
        IsTranslation = false;
    }
}
