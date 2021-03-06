﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class GameCoordinator : SingletonMonoBehaviour<GameCoordinator>
{

    bool selectedVRDevice = false;
    VRObjectManager vrObjectManager;
    [SerializeField]
    EFRNetworkManager networkManager;

    [SerializeField]
    string selectMenuStageName;

    [SerializeField]
    int[] switchIDs;

    GameObject[] switchObjects;

    Vector3[] switchDefaultLocalPos;

    public StageData playingStageData;

    Camera2DController camera2D;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(GameStartCoroutine());
        vrObjectManager = VRObjectManager.GetInstance();
        this.GetGameObjectWithCoroutine(
            CameraUtility.Camera2DName
            , x => camera2D = x.GetComponent<Camera2DController>()
            );
    }

    void OnGUI()
    {
        if (!selectedVRDevice)
        {
            GUI.TextField(new Rect(10, 10, 150, 30), "Select VR Device");

            if (GUI.Button(new Rect(10, 50, 150, 30), "No Device"))
            {
                SelectVRDevice(VRDeviceType.NO_DEVICE);
            }
            if (GUI.Button(new Rect(10, 90, 150, 30), "Oculus"))
            {
                SelectVRDevice(VRDeviceType.OCULUS);
            }
            if (GUI.Button(new Rect(10, 130, 150, 30), "HTC Vive"))
            {
                SelectVRDevice(VRDeviceType.HTC_VIVE);
            }
        }
    }


    void OnStartStageChange()
    {
        if(PlayerManager.LocalPlayer)PlayerManager.playerMove.isReady = false;
        if (PlayerManager.LocalPlayer) PlayerManager.playerStatus.isReady = false;
    }
    public void ChangeStageSelectMenu()
    {
        if (!IsChangingStage)
        {
            ChangeStage(selectMenuStageName);
            ResetSwitchsLocalPos();
        }
    }
    public void ChangeStage(string sceneName)
    {
        OnStartStageChange();
        StartCoroutine(ChangeStageCoroutine(sceneName));
    }
    public bool IsChangingStage
    {
        get;
        private set;
    }

    IEnumerator ChangeStageCoroutine(string sceneName)
    {
        IsChangingStage = true;
        FadeInOutController.VRFadePanel.StartBlackFadeOut(0.1f);
        FadeInOutController._2DFadePanel.StartBlackFadeOut(0.1f);

        //unLoad
        yield return StartCoroutine(StageSceneLoader.GetInstance().UnLoadCurrentStageScene());

        DebugTools.Log("unLo");
        //clientとserverでunloadしたことを確認
        yield return new WaitUntil(() => {
           return("" == EFRNetworkManager.curretStageName[0] &&
             "" == EFRNetworkManager.curretStageName[1]);
        });
        Debug.Log("Load");
        DebugTools.Log("Load");
        //Load
        yield return StartCoroutine(StageSceneLoader.GetInstance().LoadStageScene(sceneName));

        //clientとserverでloadしたことを確認
        yield return new WaitUntil(() => sceneName == EFRNetworkManager.curretStageName[0] &&
            sceneName == EFRNetworkManager.curretStageName[1]);

        DebugTools.Log("Ready");
        if (networkManager.isHost)NetworkServer.SetAllClientsNotReady();
        NetworkStageNameStorage.instance.CmdIamReady(PlayerManager.LocalPlayer.GetComponent<NetworkIdentity>());
         if (networkManager.isHost)
        {
            NetworkServer.SpawnObjects();
            DebugTools.Log("スポーンオブジェ");
        }
        
        //上手くFind出来ないことがあるため待つ。
        yield return new WaitForSeconds(0.5f);
        StageChangeOnEnd();
        //ガクっとカメラが切り替わると違和感があるので黒幕で隠す
        yield return new WaitForSeconds(0.1f);
        FadeInOutController._2DFadePanel.StartBlackFadeIn(1.0f);
        FadeInOutController.VRFadePanel.StartBlackFadeIn(1.0f);

        SoundManager.GetInstance().PlayStageBGM(sceneName);

        IsChangingStage = false;
    }
    void SelectVRDevice(VRDeviceType deviceType)
    {
        vrObjectManager.SetDeviceType(deviceType);
        selectedVRDevice = true;
    }
    void SetSceneIds(Scene scene)
    {
        var gameObjects=scene.GetRootGameObjects();
        foreach (var i in gameObjects)
        {
            var setters=i.transform.GetComponentsInChildren<SceneIDSetter>(true);
            foreach (var k in setters )
            {
                k.SetForceSceneID();
            }
        }
    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitUntil(() => selectedVRDevice);
        // VR機器種、選択完了
        //シーンIDセット
        SetSceneIds(SceneManager.GetSceneByName("Root_Common"));

        yield return StartCoroutine(SceneLoader.IELoadScene("Root_UI"));
        SetSceneIds(SceneManager.GetSceneByName("Root_UI"));

        yield return StartCoroutine(SceneLoader.IELoadScene("Root_Frame3D"));
        //シーンIDセット
        SetSceneIds(SceneManager.GetSceneByName("Root_Frame3D"));

        yield return StartCoroutine(SceneLoader.IELoadScene("Root_Stage"));
        yield return StartCoroutine(StageSceneLoader.GetInstance().LoadStageScene(selectMenuStageName));


        // 最初のステージロード完了
        
        vrObjectManager.SpawnVRCamObject();
        networkManager.gameObject.SetActive(true);
        // ネットワーク接続

        yield return new WaitUntil(() => networkManager.IsClientSceneReady());
        yield return new WaitUntil(() => networkManager.IsClientConnected());

        // ネットワーク接続完了


        // プレイヤースポーン
        networkManager.SpawnPlayer();
		//NetworkServer.Spawn(EffectManager.GetInstance().gameObject);
        yield return new WaitForSeconds(1f);
        // プレイヤースポーン完了
        yield return new WaitUntil(() => PlayerManager.LocalPlayer != null);

        InitilalizeSwitchs();
        
        StageChangeOnEnd();

    }

    void InitilalizeSwitchs()
    {
        switchObjects = new GameObject[switchIDs.Length];
        switchDefaultLocalPos = new Vector3[switchIDs.Length];

        int num=0;
        foreach (var i in switchIDs)
        {
            switchObjects[num]= (GimmickManager.GetActor(i) as MonoBehaviour).gameObject;
            switchDefaultLocalPos[num] = switchObjects[num].transform.localPosition;
            num++;
        }
    }

    void ResetSwitchsLocalPos()
    {
        int num = 0;
        foreach (var i in switchObjects)
        {
            i.transform.localPosition=switchDefaultLocalPos[num];
            num++;
        }
    }

    public void StartGameOverPerformance()
    {
        StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        var cameraVRController=GameObject.Find(CameraUtility.CameraVRName).GetComponent<CameraVRController>();
        cameraVRController.NoiseActivate(0.7f,1.0f);
        yield return new WaitForSeconds(1.0f);
        ChangeStageSelectMenu();
    }
    public void StageChangeOnEnd()
    {
        PlayerManager.playerMove.StageInit();
        PlayerManager.playerStatus.StageInit();
        GameObject.Find("GameOverManager").GetComponent<GameOverManager>().Init();
        vrObjectManager.InitVRCamObject();

        Debug.Log("ChangeStage End");

    }
}
