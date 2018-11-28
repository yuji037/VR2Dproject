using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(TransmissionLine))]
public class LineCreator:Editor
{
    [MenuItem("GameObject/TransmissionLine", priority = 21)]
    static void CreateLine()
    {
        //var lineLinker = target as LineLineker;
        //var pos = lineLinker.transform.position;

        var lineObj = Resources.Load("Stage/TransmissionLine");
        var lineObjCopy = GameObject.Instantiate(lineObj, Selection.activeTransform) as GameObject;
        lineObjCopy.transform.localPosition = Vector3.zero;
        //lineObjCopy.transform.position = pos;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("ポイント追加"))
        {
            var line = target as TransmissionLine;
            line.AddPoint();
        }
    }
}
#endif