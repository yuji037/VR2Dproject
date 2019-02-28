using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameOverManager : NetworkBehaviour {

    [SerializeField]
    Image _2DgameOverPanel;
    [SerializeField]
    Image _2DgameOverReturnPanel;

    FadeInOutController.FadePanel fadeGameOverPanel;
    FadeInOutController.FadePanel fadeGameOverReturnPanel;

    static GameOverManager instance = null;

    [SerializeField]
    LayerMask gameOverCameraCullingMask;
    [SerializeField]
    CameraClearFlags gameOverCameraClearFlags;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError(this + "２つ目！");
        }
        instance = this;
        Init();
    }

    public static GameOverManager GetInstance()
    {
        return instance;
    }

    // Use this for initialization
    void Init () {
        fadeGameOverPanel = new FadeInOutController.FadePanel(this, _2DgameOverPanel);
        fadeGameOverReturnPanel = new FadeInOutController.FadePanel(this, _2DgameOverReturnPanel);
    }
	
    [Command]
    public void CmdGameOver()
    {
        SoundManager.GetInstance().FadeoutBGM();

        RpcGameOver();
    }

    [ClientRpc]
    public void RpcGameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        var playerMove = PlayerManager.playerMove;
        var moveType = playerMove.moveType;
        playerMove.canMove = false;

        switch (moveType){
            case PlayerMove.MoveType._2D:

                var camera2DController = GameObject.Find(CameraUtility.Camera2DName).GetComponent<Camera2DController>();
                camera2DController.NoiseActivate(0.5f, 1.0f);
                yield return new WaitForSeconds(1.0f);

                SoundManager.GetInstance().PlayBGM("gameover");
                fadeGameOverPanel.StartWhiteFadeIn(1f);
                yield return new WaitForSeconds(3f);
                fadeGameOverReturnPanel.StartWhiteFadeIn(0f);
                yield return new WaitForSeconds(0.5f);
                

                break;
            case PlayerMove.MoveType.FIXED:

                var cameraVRController = GameObject.Find(CameraUtility.CameraVRName).GetComponent<CameraVRController>();
                cameraVRController.NoiseActivate(0.7f, 1.0f);
                yield return new WaitForSeconds(1.0f);

                SoundManager.GetInstance().PlayBGM("gameover");
                var gameOverPanel = VRObjectManager.GetInstance().VRCamObject.FindFirstChildByName("GameOverPanel");
                var gameOverReturnPanel = VRObjectManager.GetInstance().VRCamObject.FindFirstChildByName("GameOverReturnPanel");
                yield return StartCoroutine(FadeVRPanel(gameOverPanel, true, 1f));
                yield return new WaitForSeconds(3f);
                yield return StartCoroutine(FadeVRPanel(gameOverReturnPanel, true, 0f));
                yield return new WaitForSeconds(0.5f);

                break;
        }

        PlayerManager.playerStatus.CmdHoloFade(true, 1000f);

        SoundManager.GetInstance().FadeoutBGM();

        if (isServer)
            RpcGameEnd();
    }

    public IEnumerator FadeVRPanel(GameObject panel, bool fadeIn, float duration = 1f)
    {
        var mat = panel.GetComponent<MeshRenderer>().material;
        yield return StartCoroutine(FadeMaterial(mat, fadeIn, duration));
    }

    IEnumerator FadeMaterial(Material mat, bool fadeIn, float duration = 1f)
    {
        var c = mat.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            mat.color = new Color(c.r, c.g, c.b, (fadeIn ? t : (duration - t)) / duration);
            yield return null;
        }
        mat.color = new Color(c.r, c.g, c.b, fadeIn ? 1f : 0f);
    }

    [ClientRpc]
    void RpcGameEnd()
    {
        TVSwitch.IsOn = false;
        GameCoordinator.GetInstance().ChangeStageSelectMenu();
        var cameraVRAdjuster = GameObject.Find(CameraUtility.CameraVRName).GetComponent<CameraVRAdjuster>();
        cameraVRAdjuster.ChangeVRCamParamToDefault();
    }
}
