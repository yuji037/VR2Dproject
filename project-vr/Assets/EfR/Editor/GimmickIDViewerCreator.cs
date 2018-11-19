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
        var gimmicks = Object.FindObjectsOfType(typeof(GimmickBase));
        List<int> idList = new List<int>();
        List<int> overLapList = new List<int>();
        foreach (GimmickBase i in gimmicks)
        {
            if (idList.Contains(i.GimmickID))
            {
                overLapList.Add(i.GimmickID);
            }
            else
            {
                idList.Add(i.GimmickID);
            }
        }
        foreach (GimmickBase i in gimmicks)
        {
            //ヒエラルキー上にあるかチェック
            bool inHierarchy = i.gameObject.activeInHierarchy;
            //既にDebugGimmickIDViewerが生成されているかチェック
            bool viewerIsContained = i.GetComponentInChildren<DebugGimmickIDViewer>();

            if (inHierarchy && !viewerIsContained)
            {
                var textColor = Color.white;
                //重複している場合赤色で表示
                if (overLapList.Contains(i.GimmickID))
                {
                    Debug.Log("GimmickID[" + i.GimmickID + "]が重複しています");
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
