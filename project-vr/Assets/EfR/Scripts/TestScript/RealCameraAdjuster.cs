using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCameraAdjuster : MonoBehaviour
{

    Vector3 startPos;

    [SerializeField]
    Vector3 endPos;

    [SerializeField]
    RenderTexture targetTexture;


    [SerializeField]
    Camera realCam;

    Camera gameCam;
    Camera GameCam
    {
        get
        {
            if (!gameCam)
            {
                gameCam = GameObject.Find("GameCamera").GetComponent<Camera>();
            }
            return gameCam;
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

    CameraClearFlags defaultRealFlags;

    int defaultRealCullingMask;



    UnityEngine.PostProcessing.PostProcessingBehaviour postProcessing;
    private void Start()
    {
        startPos = transform.position;
        postProcessing = realCam.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        defaultRealCullingMask = realCam.cullingMask;
        defaultRealFlags = realCam.clearFlags;
        
    }


    //Real→Virtualに移行
    public void StartToVirtual()
    {
        postProcessing.enabled = true;
        StartCoroutine(ToVirtual());
    }
    IEnumerator ToVirtual()
    {
        yield return MoveThis(startPos, endPos);
        SkyCam.transform.eulerAngles = Vector3.zero;
    }
    public void EndToVirtual()
    {
        //ゲームカメラにリアルカメラのポジションを合わせる
        transform.position = GameCam.transform.position;
        transform.eulerAngles = GameCam.transform.eulerAngles /*+ realCam.transform.eulerAngles*/;

        //gameCamと同じものを見る設定
        realCam.cullingMask = GameCam.cullingMask;
        realCam.clearFlags = CameraClearFlags.Skybox;


        
        //skyCamのターゲットを外す
        SkyCam.targetTexture = null;


        //GetComponent<ControlCameraOVRRig>().enabled = true;

        postProcessing.enabled = false;
    }


    //Virtual→Realに移行
    public void StartToReal()
    {
        postProcessing.enabled = true;

        //skyCamのターゲット(TV画面)を入れなおす
        SkyCam.targetTexture = targetTexture;
        

        //realCamのdefaultの設定に
        realCam.cullingMask = defaultRealCullingMask;
        realCam.clearFlags = defaultRealFlags;
        transform.position = endPos;
      

        //コントロールを無効に
        //GetComponent<ControlCamera>().enabled = false;

     
    }
    IEnumerator ToReal()
    {

        //gameCamを平行投影にすると180度回転するので再度回転させる
        SkyCam.transform.eulerAngles = new Vector3(0, 0, 180);
        yield return StartCoroutine(MoveThis(endPos, startPos));

    }
    //RealCamが最初の位置に戻った時
    public void EndToReal()
    {
        StartCoroutine(ToReal());
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
}
