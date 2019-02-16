using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour {

	[SerializeField]
	string parentName;

    [SerializeField]
    GameObject inactiveObjInPlayMode;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init()
	{
        // コントローラの試作モデルは非表示
        if (inactiveObjInPlayMode)
            inactiveObjInPlayMode.SetActive(false);
	}

	public void SetParent()
	{
		transform.parent = GameObject.Find(parentName).transform;
	}
}
