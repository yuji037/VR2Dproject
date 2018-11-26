using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LineCreator 
{
    [MenuItem("GameObject/TransmissionLine",priority =21)]
    static void CreateLine()
    {
        //var lineLinker = target as LineLineker;
        //var pos = lineLinker.transform.position;
        
        var lineObj = Resources.Load("Stage/TransmissionLine");
        var lineObjCopy = GameObject.Instantiate(lineObj, Selection.activeTransform) as GameObject;
        lineObjCopy.transform.localPosition = Vector3.zero;
        //lineObjCopy.transform.position = pos;
    }
}
