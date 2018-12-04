﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class GimmickObjectGenerator : Editor
{
    static string assetPath = "Assets/EfR/Prefabs/Gimmick/";
    static GameObject InstantiateGimmick(string prefabName)
    {
        GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath + prefabName);
        var prefabCopy = PrefabUtility.InstantiatePrefab(prefabObject) as GameObject;
        if (Selection.activeGameObject!=null)
        {
            prefabCopy.transform.parent = Selection.activeGameObject.transform;
        }
        Selection.activeGameObject = prefabCopy;
            prefabCopy.transform.localPosition = Vector3.zero;
        Undo.RegisterCreatedObjectUndo(prefabCopy, "Create" + prefabName);
        return prefabCopy;
    }

    
    static void InstantiateAndSetGimmickID(string prefabName, int minNeedID = 1000)
    {
        var obj = InstantiateGimmick(prefabName);
        var gb = obj.GetComponent<GimmickBase>();
        Undo.RecordObject(gb, "RegistGimmickID" + prefabName);
        gb.SetGimmickID(GimmickIDManager.GetNotOverLapGimmickID(minNeedID));
    }
    [MenuItem("GameObject/EFR_Gimmicks/TransmissionLine", priority = 21)]
    static void CreateLine()
    {
        InstantiateGimmick("TransmissionLine.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmicks/GimmickCube", priority = 22)]
    static void CreateGimmickCube()
    {
        InstantiateGimmick("GimmickCube.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmicks/GimmickSwitch", priority = 23)]
    static void CreateGimmickSwitch()
    {
        InstantiateAndSetGimmickID("GimmickSwitch.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmicks/GimmickDoor", priority = 24)]
    static void CreateGimmickDoor()
    {
        InstantiateAndSetGimmickID("GimmickDoor.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmicks/GimmickWorldFlipFlop", priority = 25)]
    static void CreateGimmickWorldFlipFlop()
    {
        InstantiateAndSetGimmickID("GimmickWorldFlipFlop.prefab");
    }

}
#endif