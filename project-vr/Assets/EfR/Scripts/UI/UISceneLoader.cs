using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLoader : MonoBehaviour {

    [SerializeField]
    string firstLoadScene;

	// Use this for initialization
	void Start () {
        SceneLoader.AsyncLoadScene(firstLoadScene);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
