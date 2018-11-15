using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSceneLoader : MonoBehaviour {

    string nowSceneName;

    [SerializeField]
    string[] sceneNames;

    int sceneCount = 0;

    // Use this for initialization
    void Start()
    {
        LoadNextStage();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.H) )
        {
            if ( SceneLoader.DestroyScene(nowSceneName) )
            {
                LoadNextStage();
            }
        }
    }

    void LoadNextStage()
    {
        if(QuickStageStarter.firstStageName != "" )
        {
            LoadStageScene(QuickStageStarter.firstStageName);
            QuickStageStarter.firstStageName = "";
            return;
        }

        LoadStageScene(sceneNames[sceneCount]);
        sceneCount++; if ( sceneCount >= sceneNames.Length ) sceneCount = 0;
    }

    //IEnumerator CoLoadNextStage()
    //{
    //    yield return SceneLoader.LoadScene("");
    //}

    void LoadStageScene(string _sceneName)
    {
        var ao = SceneLoader.AsyncLoadScene(_sceneName);
        ao.completed += (_ao) =>
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(_sceneName);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
        };
        nowSceneName = _sceneName;
    }
}
