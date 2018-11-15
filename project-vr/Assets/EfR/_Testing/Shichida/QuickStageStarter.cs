using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickStageStarter : MonoBehaviour {

    public static string firstStageName = "";

    // Use this for initialization
    void Start() {
        if ( SceneManager.sceneCount > 1 )
        {
            // Root_Commonを経て呼び出されてるはずなので、QuickStartは必要ない
            return;
        }

        firstStageName = SceneManager.GetSceneAt(0).name;
        //var ao = SceneManager.UnloadSceneAsync(firstStageName);
        //ao.completed += (_ao) => { SceneLoader.AsyncLoadScene("Root_Common"); };
        SceneManager.LoadScene("Root_Common");
}
}
