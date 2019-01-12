using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSceneLoader : SingletonMonoBehaviour<StageSceneLoader> {
[SerializeField]
    string selectMenuSceneName;
    public IEnumerator LoadSelectMenuStageScene()
    {
        yield return LoadStageScene(selectMenuSceneName);
    }
    public IEnumerator LoadStageScene(string currentLoadStageName)
    {
        if (QuickStageStarter.firstStageName != "")
        {
            yield return StartCoroutine(DirectLoadStageScene(QuickStageStarter.firstStageName));
            QuickStageStarter.firstStageName = "";
            yield break;
        }
        yield return StartCoroutine(DirectLoadStageScene(currentLoadStageName));
    }

    IEnumerator DirectLoadStageScene(string _sceneName)
    {
        yield return SceneLoader.IELoadScene(_sceneName);
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(_sceneName);
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
    }
}
