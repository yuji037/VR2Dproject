using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkStageChanger :  NetworkBehaviour{
   static NetworkStageChanger instance;

   public static NetworkStageChanger Instance
    {
        get
        {
            return instance;
        }
    } 

    private void Awake()
    {
        instance = this;
    }
    [Command]
    public void CmdChangeStage(string sceneName)
    {
        RpcClientChangeStage(sceneName);
    }
    //IEnumerator ChangeStageCoroutine(string sceneName)
    //{
    //    //serverのロードが終わるとclientをロードする。
    //    RpcClientChangeStage(sceneName);
    //}

    IEnumerator ServerChangeStageCoroutine(string sceneName)
    {
        yield return StartCoroutine(StageSceneLoader.GetInstance().LoadStageScene(sceneName));
        NetworkServer.SpawnObjects();
        DebugTools.Log("スポーンオブジェ");
        PlayerManager.LocalPlayer.GetComponent<PlayerMove>().StageInit();
        PlayerManager.LocalPlayer.GetComponent<PlayerStatus>().StageInit();
    }

    [ClientRpc]
    void RpcClientChangeStage(string sceneName)
    {
        DebugTools.Log("ChangeClientScene :"+sceneName);
        StartCoroutine(ClientChangeStageCoroutine(sceneName));
    }
    IEnumerator ClientChangeStageCoroutine(string sceneName)
    {
        yield return StartCoroutine(StageSceneLoader.GetInstance().LoadStageScene(sceneName));
        NetworkServer.SpawnObjects();
        DebugTools.Log("スポーンオブジェ");
        PlayerManager.LocalPlayer.GetComponent<PlayerMove>().StageInit();
        PlayerManager.LocalPlayer.GetComponent<PlayerStatus>().StageInit();
    }

}
