using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    // シーンの追加ロード IEnumerator型　StartCroutine(SceneLoader.LoadScene(シーン名))で呼び出し
    // Update内で簡単に呼び出し可能
    public static IEnumerator IELoadScene(string _sceneName)
    {
        yield return SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
    }

    // シーンの追加ロード AsyncOperation型
    public static AsyncOperation AsyncLoadScene(string _sceneName)
    {
        return SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
    }

    // シーンの追加ロード AsyncOperation型 allowSceneActivationをtrueにするまでシーンの追加は行われない関数
    // 第二引数を明示的にtrueにすれば読み込み完了時すぐにシーンを追加する
    public static AsyncOperation AsyncLoadScene(string _sceneName, bool _allowSceneActivation = false)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        async.allowSceneActivation = _allowSceneActivation;
        return async;
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
        Debug.LogError("Can't Destroy Scene" + _sceneName);
       return false;
    }
}
