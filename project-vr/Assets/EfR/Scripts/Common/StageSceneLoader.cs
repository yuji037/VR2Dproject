using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSceneLoader : SingletonMonoBehaviour<StageSceneLoader> {

    string nowSceneName;

    [SerializeField]
    string[] sceneNames;

    int sceneCount = 0;

    //// Use this for initialization
    //void Start()
    //{
    //    StartCoroutine(LoadNextStage());
    //}

    // Update is called once per frame
    void Update()
    {
        //if ( Input.GetKeyDown(KeyCode.H) )
        //{
        //    if ( SceneLoader.DestroyScene(nowSceneName) )
        //    {
        //        StartCoroutine(LoadNextStage());
        //    }
        //}
    }

    public IEnumerator LoadNextStage()
    {
        if(QuickStageStarter.firstStageName != "" )
        {
            yield return StartCoroutine(LoadStageScene(QuickStageStarter.firstStageName));
            QuickStageStarter.firstStageName = "";
            yield break;
        }

        yield return StartCoroutine(LoadStageScene(sceneNames[sceneCount]));
        sceneCount++; if ( sceneCount >= sceneNames.Length ) sceneCount = 0;
    }

    IEnumerator LoadStageScene(string _sceneName)
    {
        yield return SceneLoader.IELoadScene(_sceneName);
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(_sceneName);
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
        nowSceneName = _sceneName;
    }
}
