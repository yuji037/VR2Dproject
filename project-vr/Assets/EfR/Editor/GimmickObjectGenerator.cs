using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
#if UNITY_EDITOR
using UnityEditor;
public class GimmickObjectGenerator : Editor
{
    static string gimmickPath = "Assets/EfR/Prefabs/Gimmick/";
    static string screenPath = "Assets/EfR/Prefabs/Screen/";
    static string cameraPath = "Assets/EfR/Prefabs/Camera/";
    static string primitivePath = "Assets/EfR/Prefabs/Primitive/";

    static GameObject InstantiatePrefab(string prefabPath, bool isSelectPrefab = true)
    {
        GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var prefabCopy = PrefabUtility.InstantiatePrefab(prefabObject) as GameObject;
        if (Selection.activeGameObject != null)
        {
            prefabCopy.transform.parent = Selection.activeGameObject.transform;
        }
        if (isSelectPrefab) Selection.activeGameObject = prefabCopy;
        prefabCopy.transform.localPosition = Vector3.zero;
        Undo.RegisterCreatedObjectUndo(prefabCopy, "Create" + prefabCopy.name);
        return prefabCopy;
    }

    static T InstantiateUniqueNamePrefab<T>(string prefabPath, bool isSelectPrefab = true)
        where T : MonoBehaviour
    {
        var obj = InstantiatePrefab(prefabPath, isSelectPrefab);
        Undo.RecordObject(obj, "Rename" + prefabPath);
        obj.name = GenerateUniqueObjectName(typeof(T), obj.name);
        return obj.GetComponent<T>();
    }

    static void InstantiateAndSetGimmickID(string prefabPath, int minNeedID = 1000)
    {
        var obj = InstantiatePrefab(prefabPath);
        var gb = obj.GetComponent<GimmickBase>();
        Undo.RecordObject(gb, "RegistGimmickID" + prefabPath);
        gb.SetGimmickID(GimmickIDManager.GetNotOverLapGimmickID(minNeedID));
    }

  

    [MenuItem("GameObject/EFR_Gimmick/TransmissionLine", priority = 21)]
    static void CreateLine()
    {
        InstantiatePrefab(gimmickPath + "TransmissionLine.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/GimmickCube", priority = 22)]
    static void CreateGimmickCube()
    {
        InstantiatePrefab(gimmickPath + "GimmickCube.prefab");
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
        InstantiatePrefab(screenPath + "NormalFloorScreen.prefab");
    }

    [MenuItem("GameObject/EFR_Screen/LongX", priority = 27)]
    static void CreateLongXFloorScreen()
    {
        InstantiatePrefab(screenPath + "LongXFloorScreen.prefab");
    }

    [MenuItem("GameObject/EFR_Screen/LongY", priority = 28)]
    static void CreateLongYFloorScreen()
    {
        InstantiatePrefab(screenPath + "LongYFloorScreen.prefab");
    }


    [MenuItem("GameObject/EFR_Gimmick/SavePoint", priority = 29)]
    static void CreateSavePoint()
    {
        InstantiatePrefab(gimmickPath + "SavePoint.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/RespawnFloor", priority = 30)]
    static void CreateRespawnFloor()
    {
        InstantiatePrefab(gimmickPath + "RespawnFloor.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/Spring", priority = 30)]
    static void CreateSpring()
    {
        InstantiatePrefab(gimmickPath + "Spring.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/Dossun", priority = 30)]
    static void CreateDossun()
    {
        InstantiatePrefab(gimmickPath + "Dossun.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/DollyFloor", priority = 30)]
    static void CreateDollyFloor()
    {
        var path = InstantiateUniqueNamePrefab<CinemachineSmoothPath>(gimmickPath + "LoopDollyTrack.prefab");
        var dollyFloorObj=InstantiatePrefab(gimmickPath + "DollyFloor.prefab");
        var dollyFloor = dollyFloorObj.GetComponent<DollyFloor>();
        Undo.RecordObject(dollyFloor, "create track");
        dollyFloor.path= path;
    }


    //カメラ系
    [MenuItem("GameObject/EFR_Gimmick/VRCamChangeBox", priority = 30)]
    static void CreateVirtualCameraChanger()
    {
        InstantiatePrefab(gimmickPath + "VRCamChangeBox.prefab");
    }

    [MenuItem("GameObject/EFR_Gimmick/2DCamParamChangeBox", priority = 30)]
    static void Create2DCamOffsetChanger()
    {
        InstantiatePrefab(gimmickPath + "2DCamParamChangeBox.prefab");
    }

    [MenuItem("GameObject/EFR_Camera/FixedVcam", priority = 30)]
    static void CreateFixedCamera()
    {
        InstantiateUniqueNamePrefab<CinemachineVirtualCamera>(cameraPath + "FixedVcam.prefab");
    }

    [MenuItem("GameObject/EFR_Camera/DollyVcam", priority = 30)]
    static void CreateDollyCamera()
    {
        var vcam = InstantiateUniqueNamePrefab<CinemachineVirtualCamera>(cameraPath + "DollyVcam.prefab", false);
        var path = InstantiateUniqueNamePrefab<CinemachineSmoothPath>(cameraPath + "DollyTrack.prefab");
        var dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        Undo.RecordObject(dolly, "create track");
        dolly.m_Path = path;
    }

    [MenuItem("GameObject/EFR_Camera/2DVcam", priority = 30)]
    static void Create2DCamera()
    {
        InstantiateUniqueNamePrefab<CinemachineVirtualCamera>(cameraPath + "2DVcam.prefab");
    }

    public static string GenerateUniqueObjectName(Type type, string prefix)
    {
        int count = 0;
        UnityEngine.Object[] all = Resources.FindObjectsOfTypeAll(type);
        foreach (UnityEngine.Object o in all)
        {
            if (o != null && o.name.StartsWith(prefix))
            {
                string suffix = o.name.Substring(prefix.Length);
                int i;
                if (Int32.TryParse(suffix, out i) && i > count)
                    count = i;
            }
        }
        return prefix + (count + 1);
    }


    //primitive
    [MenuItem("GameObject/Primitive/GlowCube",priority =30)]
    static void CreateGlowCube()
    {
        InstantiatePrefab(primitivePath+"GlowCube.prefab");
    }
}
#endif