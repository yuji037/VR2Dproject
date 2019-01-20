using System.Collections;
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

    public GameObject[] GetFloorPrefabObjects
    {
        get { return FloorPrefabObjects; }
    }

    [Command]
    void CmdSpawnFloor(int floorForm,NetworkIdentity playerNetID)
    {
        var obj = Instantiate(FloorPrefabObjects[(floorForm)]);
        obj.transform.position = new Vector3(0,-1000.0f,0);
        NetworkServer.Spawn(obj);
        obj.GetComponent<NetworkIdentity>().AssignClientAuthority(playerNetID.connectionToClient);
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

    public void ReleaseFloor(GimmickFloor floor)
    {
        var floorID = floor.netId;
        CmdDestroyFloor(floorID);
    }

    [Command]
    void CmdDestroyFloor(NetworkInstanceId floorID)
    {
        NetworkServer.Destroy(NetworkServer.FindLocalObject(floorID));
    }
    IEnumerator SpawnFloorCoroutine(FloorForm floorForm, System.Action<GimmickFloor> callBack)
    {
        CmdSpawnFloor((int)floorForm,PlayerManager.LocalPlayer.GetComponent<NetworkIdentity>());
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
