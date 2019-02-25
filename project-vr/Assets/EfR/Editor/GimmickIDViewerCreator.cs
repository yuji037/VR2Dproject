using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GImmickIDViewerCreator
{

    [MenuItem("デバッグツール/GimmickIDを表示")]
    static void CreateIDViewerInstance()
    {
        var obj = Resources.Load("Debug/GimmickIDText") as GameObject;
        var components = Object.FindObjectsOfType(typeof(Component));
        List<int> idList = new List<int>();
        List<int> overLapList = new List<int>();
        List<IActor> actors = new List<IActor>();
        foreach (var i in components)
        {
            if (!(i is IActor)) continue;
            actors.Add(i as IActor);
        }
        foreach (IActor i  in actors)
        {
            if (idList.Contains(i.GetID()))
            {
                overLapList.Add(i.GetID());
            }
            else
            {
                idList.Add(i.GetID());
            }
        }
        foreach (IActor i in actors)
        {
            var mono = i as MonoBehaviour;
            //ヒエラルキー上にあるかチェック
            bool inHierarchy = mono.gameObject.activeInHierarchy;
            //既にDebugGimmickIDViewerが生成されているかチェック
            bool viewerIsContained = mono.GetComponentInChildren<DebugGimmickIDViewer>();

            if (inHierarchy && !viewerIsContained)
            {
                var textColor = Color.white;
                //重複している場合赤色で表示
                if (overLapList.Contains(i.GetID()))
                {
                    Debug.Log("GimmickID[" + i.GetID() + "]が重複しています");
                    textColor = Color.red;
                }
                var textObject = Object.Instantiate(obj);
                textObject.GetComponent<DebugGimmickIDViewer>().Init(i,textColor);

            }
        }
    }
    [MenuItem("デバッグツール/GimmickIDを非表示")]
    static void DeleteIDViewerInstance()
    {
        var viewers = Object.FindObjectsOfType(typeof(DebugGimmickIDViewer));
        foreach (DebugGimmickIDViewer i in viewers)
        {
            if (i.gameObject.activeInHierarchy)
            {
                Object.DestroyImmediate(i.gameObject);
            }
        }
    }

}
