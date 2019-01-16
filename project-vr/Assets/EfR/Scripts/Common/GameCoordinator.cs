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

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GameStartCoroutine());
        vrObjectManager = VRObjectManager.GetInstance();
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
    }
    public void ChangeStage(string sceneName)
    {
        OnStartStageChange();
        StartCoroutine(ChangeStageCoroutine(sceneName));
    }
    IEnumerator ChangeStageCoroutine(string sceneName)
    {
        FadeInOutController.GetInstance().StartBlackFadeOut(0.1f);
        //unLoad
        yield return StartCoroutine(StageSceneLoader.GetInstance().UnLoadCurrentStageScene());

        DebugTools.Log("unLo");
        //clientとserverでunloadしたことを確認
        yield return new WaitUntil(() => {
            Debug.Log(EFRNetworkManager.curretStageName[0] + "1=" + EFRNetworkManager.curretStageName[1]);
            var aa="" == EFRNetworkManager.curretStageName[0] &&
             "" == EFRNetworkManager.curretStageName[1];
            Debug.Log(aa);
            return aa;
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
        NetworkStageNameStorage.instance.CmdIamReady(NetworkStageNameStorage.instance.GetComponent<NetworkIdentity>());

         if (networkManager.isHost)
        {
            NetworkServer.SpawnObjects();
            DebugTools.Log("スポーンオブジェ");
        }
        
        //上手くFind出来ないことがあるため待つ。
        yield return new WaitForSeconds(0.5f);
        OnEndStageChange();
        //ガクっとカメラが切り替わると違和感があるので黒幕で隠す
        yield return new WaitForSeconds(0.1f);
        FadeInOutController.GetInstance().StartBlackFadeIn(1.0f);
    }
    void SelectVRDevice(VRDeviceType deviceType)
    {
        vrObjectManager.SetDeviceType(deviceType);
        selectedVRDevice = true;
    }

    IEnumerator GameStartCoroutine()
    {
        yield return new WaitUntil(() => selectedVRDevice);
        // VR機器種、選択完了

        StartCoroutine(SceneLoader.IELoadScene("Root_UI"));
        yield return StartCoroutine(SceneLoader.IELoadScene("Root_Frame3D"));
        yield return StartCoroutine(SceneLoader.IELoadScene("Root_Stage"));
        yield return StartCoroutine(StageSceneLoader.GetInstance().LoadSelectMenuStageScene());


        // 最初のステージロード完了

        vrObjectManager.SpawnVRCamObject();
        networkManager.gameObject.SetActive(true);
        // ネットワーク接続

        yield return new WaitUntil(() => networkManager.IsClientSceneReady());
        yield return new WaitUntil(() => networkManager.IsClientConnected());

        // ネットワーク接続完了

        // プレイヤースポーン
        networkManager.SpawnPlayer();
        yield return new WaitForSeconds(1f);
        // プレイヤースポーン完了
        yield return new WaitUntil(() => PlayerManager.LocalPlayer != null);

        OnEndStageChange();

    }

    public void OnEndStageChange()
    {
        PlayerManager.playerMove.StageInit();
        PlayerManager.playerStatus.StageInit();
        vrObjectManager.InitVRCamObject();

        Debug.Log("Stage End");

    }
}
