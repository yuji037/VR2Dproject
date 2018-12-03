using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectPool : SingletonMonoBehaviour<StageObjectPool> {
    [SerializeField]
    GameObject testCube;

    Dictionary<bool,GameObject> objectList=new Dictionary<bool, GameObject>(); 
	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
            objectList.Add(false,child.gameObject);
        }
	}
    public GameObject GetPoolStageObject()
    {
        return testCube;
    }
}
