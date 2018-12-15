using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StageSaver : NetworkBehaviour
{
    static StageSaver instance = null;
    private void Awake()
    {
        instance = this;
    }
    public static StageSaver GetInstance()
    {
        return instance;
    }
  
    struct SaveParam
    {
        public SaveParam(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
        public Vector3 position;
        public Quaternion rotation;
    }
    Dictionary<SavableObject, SaveParam> savableParams = new Dictionary<SavableObject, SaveParam>();

    [Server]
    public void RegisterSavableObject(SavableObject savableObject)
    {
        savableParams.Add(savableObject, new SaveParam(savableObject.transform.position, savableObject.transform.rotation));
    }

    [Server]
    public void SaveAllSavableObjects()
    {
        foreach (var i in savableParams)
        {
            savableParams[i.Key] = new SaveParam(i.Key.transform.position, i.Key.transform.rotation);
        }
    }

    [Server]
    public void LoadAllSavableObjects()
    {
        foreach (var i in savableParams)
        {
            i.Key.transform.position = i.Value.position;
            i.Key.transform.rotation = i.Value.rotation;
        }
    }

    [Server]
    public void Respawn(SavableObject savableObject)
    {
        savableObject.transform.position = savableParams[savableObject].position;
        savableObject.transform.rotation = savableParams[savableObject].rotation;
    }
}
