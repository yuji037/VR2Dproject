using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class GimmickObjectGenerator : Editor
{
    static string gimmickPath = "Assets/EfR/Prefabs/Gimmick/";
    static string screenPath = "Assets/EfR/Prefabs/Screen/";

    static GameObject InstantiateGimmick(string prefabPath)
    {
        GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var prefabCopy = PrefabUtility.InstantiatePrefab(prefabObject) as GameObject;
        if (Selection.activeGameObject != null)
        {
            prefabCopy.transform.parent = Selection.activeGameObject.transform;
        }
        Selection.activeGameObject = prefabCopy;
        prefabCopy.transform.localPosition = Vector3.zero;
        Undo.RegisterCreatedObjectUndo(prefabCopy, "Create" + prefabCopy.name);
        return prefabCopy;
    }


    static void InstantiateAndSetGimmickID(string prefabPath, int minNeedID = 1000)
    {
        var obj = InstantiateGimmick(prefabPath);
        var gb = obj.GetComponent<GimmickBase>();
        Undo.RecordObject(gb, "RegistGimmickID" + prefabPath);
        gb.SetGimmickID(GimmickIDManager.GetNotOverLapGimmickID(minNeedID));
    }
    [MenuItem("GameObject/EFR_Gimmick/TransmissionLine", priority = 21)]
    static void CreateLine()
    {
        InstantiateGimmick(gimmickPath + "TransmissionLine.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/GimmickCube", priority = 22)]
    static void CreateGimmickCube()
    {
        InstantiateGimmick(gimmickPath + "GimmickCube.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/GimmickSwitch", priority = 23)]
    static void CreateGimmickSwitch()
    {
        InstantiateAndSetGimmickID(gimmickPath + "GimmickSwitch.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/GimmickDoor", priority = 24)]
    static void CreateGimmickDoor()
    {
        InstantiateAndSetGimmickID(gimmickPath + "GimmickDoor.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/GimmickWorldFlipFlop", priority = 25)]
    static void CreateGimmickWorldFlipFlop()
    {
        InstantiateAndSetGimmickID(gimmickPath + "GimmickWorldFlipFlop.prefab");
    }
    [MenuItem("GameObject/EFR_Screen/Normal", priority = 26)]
    static void CreateNoramlFloorScreen()
    {
        InstantiateGimmick(screenPath + "NormalFloorScreen.prefab");
    }

    [MenuItem("GameObject/EFR_Screen/LongX", priority = 27)]
    static void CreateLongXFloorScreen()
    {
        InstantiateGimmick(screenPath + "LongXFloorScreen.prefab");
    }

    [MenuItem("GameObject/EFR_Screen/LongY", priority = 28)]
    static void CreateLongYFloorScreen()
    {
        InstantiateGimmick(screenPath + "LongYFloorScreen.prefab");
    }

}
#endif