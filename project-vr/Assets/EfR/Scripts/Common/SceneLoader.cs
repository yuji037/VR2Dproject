using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    // シーンの追加ロード　StartCroutine(SceneLoader.LoadScene(シーン名))で呼び出し
    public static IEnumerator LoadScene(string _sceneName)
    {
        yield return SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
    }

    // シーンの破棄
    public static bool DestroyScene(string _sceneName)
    {
       for (int aCnt = 0; aCnt < SceneManager.sceneCount; ++aCnt)
        {
            Scene scene = SceneManager.GetSceneAt(aCnt);
            if (scene.name == _sceneName)
            {
                SceneManager.UnloadSceneAsync(_sceneName);
                return true;
            }
        }
        return false;
    }
}
