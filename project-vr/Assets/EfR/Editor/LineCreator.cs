using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(TransmissionLine))]
public class LineCreator:Editor
{

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