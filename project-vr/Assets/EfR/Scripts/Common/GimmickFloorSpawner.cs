﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickFloorSpawner : NetworkBehaviour
{

    static GimmickFloorSpawner instance;
    public static GimmickFloorSpawner GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    Dictionary<int, GimmickFloor> floorDictionary = new Dictionary<int, GimmickFloor>();
    public void RegisterFloor(GimmickFloor floor)
    {
        if (floorDictionary.ContainsKey((int)floor.floorForm) && floorDictionary[(int)floor.floorForm] == null)
        {
            floorDictionary[(int)floor.floorForm] = floor;
        }
        else
        {
            floorDictionary.Add((int)floor.floorForm, floor);
        }
    }

    [SerializeField]
    GameObject[] FloorPrefabObjects;
    [Command]
    void CmdSpawnFloor(int floorForm,NetworkIdentity spawnerNetID,NetworkIdentity playerNetID)
    {
        var obj = Instantiate(FloorPrefabObjects[(floorForm)]);
        NetworkServer.Spawn(obj);
        obj.GetComponent<GimmickFloor>().RpcInit(playerNetID);

    }


    public void GetFloorObject(FloorForm floorForm, System.Action<GimmickFloor> callBack)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(SpawnFloorCoroutine(floorForm, callBack));
    }
    IEnumerator SpawnFloorCoroutine(FloorForm floorForm, System.Action<GimmickFloor> callBack)
    {
        CmdSpawnFloor((int)floorForm,GetComponent<NetworkIdentity>(),PlayerManager.LocalPlayer.GetComponent<NetworkIdentity>());
        while (true)
        {
            if (floorDictionary.ContainsKey((int)floorForm))
            {
                callBack(floorDictionary[(int)floorForm]);
                floorDictionary.Remove((int)floorForm);
                yield break;
            }
            yield return null;
        }
    }

}