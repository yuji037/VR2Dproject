using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class StageSceneLoader : SingletonMonoBehaviour<StageSceneLoader>
{
    [SerializeField]
    string selectMenuSceneName;

    string lastStageSceneName;

    public IEnumerator LoadSelectMenuStageScene()
    {
        yield return LoadStageScene(selectMenuSceneName);
    }

    public IEnumerator UnLoadCurrentStageScene()
    {
        yield return SceneLoader.DestroyScene(lastStageSceneName);
        NetworkStageNameStorage.instance.CmdSetCurrentStageName(PlayerManager.GetPlayerNumber(), "");
        //clientがつながっていないとき
        if (PlayerManager.Players[1] == null) NetworkStageNameStorage.instance.CmdSetCurrentStageName(1, "");

    }

    public IEnumerator LoadStageScene(string loadStageName)
    {

        if (QuickStageStarter.firstStageName != "")
        {
            yield return StartCoroutine(DirectLoadStageScene(QuickStageStarter.firstStageName));
            QuickStageStarter.firstStageName = "";
            yield break;
        }

        yield return StartCoroutine(DirectLoadStageScene(loadStageName));
    }

    IEnumerator DirectLoadStageScene(string loadStageName)
    {
        yield return SceneLoader.IELoadScene(loadStageName);
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(loadStageName);
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
        if (PlayerManager.LocalPlayer)
        {
            NetworkStageNameStorage.instance.CmdSetCurrentStageName(PlayerManager.GetPlayerNumber(), loadStageName);
            //clientがつながっていないとき
            if (PlayerManager.Players[1] == null) NetworkStageNameStorage.instance.CmdSetCurrentStageName(1, loadStageName);
        }
        lastStageSceneName = loadStageName;

    }

}
